namespace Allors.Workspace.Client
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

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

        public Session Session { get; }

        public Indexer<SessionObject> Objects { get; private set; }

        public Indexer<SessionObject[]> Collections { get; private set; }

        public Indexer<object> Values { get; private set; }

        public async Task Load(object args)
        {
            var response = await this.database.Pull(this.name, args);
            var requireLoadIds = this.workspace.Diff(response);
            if (requireLoadIds.objects.Length > 0)
            {
                var loadResponse = await this.database.Sync(requireLoadIds);
                this.workspace.Sync(loadResponse);
            }

            this.Update(response);
            this.Session.Reset();
        }

        public async Task<Result> Query(string service, object args)
        {
            var pullResponse = await this.database.Pull(service, args);
            var requireLoadIds = this.workspace.Diff(pullResponse);
            if (requireLoadIds.objects.Length > 0)
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
            if (!pushResponse.hasErrors)
            {
                this.Session.PushResponse(pushResponse);

                var objects = saveRequest.objects.Select(v => v.i).ToArray();
                if (pushResponse.newObjects != null)
                {
                    objects = objects.Union(pushResponse.newObjects.Select(v => v.i)).ToArray();
                }

                var requireLoadIds = new SyncRequest
                                         {
                                             objects = objects
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
            this.Objects = new Indexer<SessionObject>(response.namedObjects.ToDictionary(
                pair => pair.Key,
                pair => this.Session.Get(long.Parse(pair.Value))));
            this.Collections = new Indexer<SessionObject[]>(response.namedCollections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(v => this.Session.Get(long.Parse(v))).ToArray()));
            this.Values = new Indexer<object>(response.namedValues.ToDictionary(
                pair => pair.Key,
                pair => pair.Value));
        }
    }
}