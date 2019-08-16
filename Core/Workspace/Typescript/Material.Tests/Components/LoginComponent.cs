// <copyright file="LoginComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace src.app.auth
{
    using Components;
    using src.app.dashboard;

    public partial class LoginComponent
    {
        public DashboardComponent Login(string userName = "administrator")
        {
            this.Username.Value = userName;
            this.SignIn.Click();

            this.Driver.WaitForAngular();

            return new DashboardComponent(this.Driver);
        }
    }
}
