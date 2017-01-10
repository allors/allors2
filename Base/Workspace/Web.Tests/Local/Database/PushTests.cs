namespace Tests.Local.Workspace
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web.Database;

    using global::Web.Controllers;

    using global::Tests.Local;

    using NUnit.Framework;

    using Should;

    public class PushTests : ControllersTest
    {
        [Test]
        public void GuestSetUnit()
        {
            // Arrange
            var c1a = new C1Builder(this.Session)
               .WithC1AllorsString("c1")
               .WithI1AllorsString("i1")
               .WithI12AllorsString("i12")
               .Build();

            this.Session.Derive();
            this.Session.Commit();

            var pushRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1AllorsString",
                                S = "new c1"
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session };

            // Act
            var jsonResult = (JsonResult)controller.Push(pushRequest);
            var pushResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            pushResponse.HasErrors.ShouldBeTrue();
            pushResponse.AccessErrors.Count.ShouldEqual(1);

            c1a.C1AllorsString.ShouldEqual("c1");
        }

        [Test]
        public void AdministratorSetUnit()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            var c1a = new C1Builder(this.Session)
               .WithC1AllorsString("c1")
               .WithI1AllorsString("i1")
               .WithI12AllorsString("i12")
               .Build();

            this.Session.Derive();
            this.Session.Commit();

            var pushRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1AllorsString",
                                S = "new c1"
                            }   
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session,AllorsUser = administrator};

            // Act
            var jsonResult = (JsonResult)controller.Push(pushRequest);
            var pushResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            pushResponse.HasErrors.ShouldBeFalse();

            c1a.C1AllorsString.ShouldEqual("new c1");
        }

        [Test]
        public void AdministratorSetOne()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            var c1a = new C1Builder(this.Session)
               .Build();

            var c1b = new C1Builder(this.Session)
               .Build();

            this.Session.Derive();
            this.Session.Commit();

            var pushRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1C1One2One",
                                S = c1b.Id.ToString()
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(pushRequest);
            var pushResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            pushResponse.HasErrors.ShouldBeFalse();

            c1a.C1C1One2One.ShouldEqual(c1b);
        }

        [Test]
        public void AdministratorSetMany()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            var c1a = new C1Builder(this.Session)
               .Build();

            var c1b = new C1Builder(this.Session)
               .Build();

            var c1c = new C1Builder(this.Session)
               .Build();

            c1a.AddC1C1One2Many(c1b);

            this.Session.Derive();
            this.Session.Commit();

            var pushRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1C1One2Many",
                                S = new [] { c1c.Id.ToString() } 
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(pushRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            c1a.C1C1One2Manies.ShouldNotBeSameAs(new [] { c1c } );
        }

        [Test]
        public void AdministratorAddMany()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            var c1a = new C1Builder(this.Session)
               .Build();

            var c1b = new C1Builder(this.Session)
               .Build();

            var c1c = new C1Builder(this.Session)
               .Build();

            c1a.AddC1C1One2Many(c1b);

            this.Session.Derive();
            this.Session.Commit();

            var saveRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1C1One2Many",
                                A = new [] { c1c.Id.ToString() }
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(saveRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            c1a.C1C1One2Manies.ShouldNotBeSameAs(new[] { c1b, c1c });
        }

        [Test]
        public void AdministratorRemoveMany()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            var c1a = new C1Builder(this.Session)
               .Build();

            var c1b = new C1Builder(this.Session)
               .Build();

            var c1c = new C1Builder(this.Session)
               .Build();

            c1a.AddC1C1One2Many(c1b);
            c1a.AddC1C1One2Many(c1c);

            this.Session.Derive();
            this.Session.Commit();

            var saveRequest = new PushRequest
            {
                Objects = new[] {
                    new PushRequestObject
                    {
                        I = c1a.Id.ToString(),
                        V = c1a.Strategy.ObjectVersion.ToString(),
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1C1One2Many",
                                R = new [] { c1c.Id.ToString() }
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(saveRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            c1a.C1C1One2Manies.ShouldNotBeSameAs(new[] { c1b });
        }

        [Test]
        public void AdministratorNew()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            this.Session.Commit();

            var saveRequest = new PushRequest
            {
                 NewObjects = new[] {
                    new PushRequestNewObject
                    {
                        NI = "-1",
                        T = "C1",
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(saveRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            saveResponse.NewObjects.Length.ShouldEqual(1);

            var newObject = saveResponse.NewObjects[0];
            var newId = newObject.NI;
            var id = newObject.I;

            newId.ShouldEqual("-1");
            var c1 = this.Session.Instantiate(id);  
            c1.ShouldNotBeNull();
        }

        [Test]
        public void AdministratorNewSetUnit()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            this.Session.Commit();

            var saveRequest = new PushRequest
            {
                NewObjects = new[] {
                    new PushRequestNewObject
                    {
                        NI = "-1",
                        T = "C1",
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1AllorsString",
                                S = "new c1"
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(saveRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            saveResponse.NewObjects.Length.ShouldEqual(1);

            var newObject = saveResponse.NewObjects[0];
            var newId = newObject.NI;
            var id = newObject.I;

            newId.ShouldEqual("-1");
            var c1 = (C1)this.Session.Instantiate(id);
            c1.ShouldNotBeNull();

            c1.C1AllorsString = "new C1";
        }

        [Test]
        public void AdministratorNewSetNewOne()
        {
            // Arrange
            var administrator = new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName);

            this.Session.Commit();

            var saveRequest = new PushRequest
            {
                NewObjects = new[] {
                    new PushRequestNewObject
                    {
                        NI = "-1",
                        T = "C1",
                        Roles = new List<PushRequestRole>
                        {
                            new PushRequestRole
                            {
                                T = "C1C1One2One",
                                S = "-1"
                            }
                        }
                    }
                }
            };

            var controller = new DatabaseController { AllorsSession = this.Session, AllorsUser = administrator };

            // Act
            var jsonResult = (JsonResult)controller.Push(saveRequest);
            var saveResponse = (PushResponse)jsonResult.Data;

            // Assert
            this.Session.Rollback();

            saveResponse.HasErrors.ShouldBeFalse();

            saveResponse.NewObjects.Length.ShouldEqual(1);

            var newObject = saveResponse.NewObjects[0];
            var newId = newObject.NI;
            var id = newObject.I;

            newId.ShouldEqual("-1");
            var c1 = (C1)this.Session.Instantiate(id);
            c1.ShouldNotBeNull();

            c1.C1C1One2One = c1;
        }
    }
}