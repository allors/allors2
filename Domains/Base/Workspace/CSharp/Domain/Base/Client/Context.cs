namespace Allors.Workspace.Client
{
    using System.Linq;
    using System.Threading.Tasks;

    using Protocol.Remote.Invoke;
    using Protocol.Remote.Pull;
    using Protocol.Remote.Push;
    using Protocol.Remote.Sync;

    public class Context
    {
        private readonly string name;
        private readonly Database database;
        private readonly Workspace workspace;

        public Context(string name, Database database, Workspace workspace)
        {
            this.name = name;
            this.database = database;
            this.workspace = workspace;

            this.Session = new Session(this.workspace);
        }

        public Context(Context parentContext, string name)
        {
            this.name = name;
            this.database = parentContext.database;
            this.workspace = parentContext.workspace;

            this.Session = parentContext.Session;
        }

        public Session Session { get; }

        public Indexer<SessionObject> Objects { get; private set; }

        public Indexer<SessionObject[]> Collections { get; private set; }

        public Indexer<object> Values { get; private set; }

        public async Task Load(object args)
        {
            var response = await this.database.Pull(this.name, args);
            var requireLoadIds = this.workspace.Diff(response);
            if (requireLoadIds.Objects.Length > 0)
            {
                var loadResponse = await this.database.Sync(requireLoadIds);
                this.workspace.Sync(loadResponse);
            }

            this.Update(response);
        }

        public async Task<Result> Query(string service, object args)
        {
            var pullResponse = await this.database.Pull(service, args);
            var requireLoadIds = this.workspace.Diff(pullResponse);
            if (requireLoadIds.Objects.Length > 0)
            {
                var loadResponse = await this.database.Sync(requireLoadIds);
                this.workspace.Sync(loadResponse);
            }

            var result = new Result(this.Session, pullResponse);
            return result;
        }

        public async Task<PushResponse> Save()
        {
            var saveRequest = this.Session.PushRequest();
            var pushResponse = await this.database.Push(saveRequest);
            if (!pushResponse.HasErrors)
            {
                this.Session.PushResponse(pushResponse);

                var objects = saveRequest.Objects.Select(v => v.I).ToArray();
                if (pushResponse.NewObjects != null)
                {
                    objects = objects.Union(pushResponse.NewObjects.Select(v => v.I)).ToArray();
                }

                var requireLoadIds = new SyncRequest
                                         {
                                             Objects = objects
                                         };

                var loadResponse = await this.database.Sync(requireLoadIds);
                this.workspace.Sync(loadResponse);
                this.Session.Reset();
            }

            return pushResponse;
        }

        public Task<InvokeResponse> Invoke(Method method)
        {
            return this.database.Invoke(method);
        }

        public Task<InvokeResponse> Invoke(string service, object args)
        {
            return this.database.Invoke(service, args);
        }
        
        private void Update(PullResponse response)
        {
            this.Objects = new Indexer<SessionObject>(response.NamedObjects.ToDictionary(
                pair => pair.Key,
                pair => this.Session.Get(long.Parse(pair.Value))));
            this.Collections = new Indexer<SessionObject[]>(response.NamedCollections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(v => this.Session.Get(long.Parse(v))).ToArray()));
            this.Values = new Indexer<object>(response.NamedValues.ToDictionary(
                pair => pair.Key,
                pair => pair.Value));
        }
    }
}