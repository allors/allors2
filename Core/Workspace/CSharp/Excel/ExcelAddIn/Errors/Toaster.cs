namespace WindowsForms.ToastNotifications
{
    using System.Drawing;
    using System.Text;
    using global::ToastNotifications;
    using Allors.Protocol.Remote;

    public static class Toaster
    {
        public static void ShowToast(string message, int duration = 2)
        {
            var toastNotification = new Notification("Message", message, duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.BackColor = Color.LightGreen;
            toastNotification.Title.ForeColor = Color.Black;
            toastNotification.Show();
        }

        public static void Warning(string message, int duration = -1)
        {
            var toastNotification = new Notification("Message", message, duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.BackColor = Color.Orange;
            toastNotification.Title.ForeColor = Color.Black;
            toastNotification.Show();
        }

        public static void Info(string title, string message, int duration = -1)
        {
            var toastNotification = new Notification(title, message, duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.BackColor = Color.LightGreen;
            toastNotification.Title.ForeColor = Color.Black;
            toastNotification.Body.ForeColor = Color.Black;

            toastNotification.Show();
        }


        public static void ConcurrencyDetectedToast(int duration = -1)
        {
            var toastNotification = new Notification("Concurrency Error", @"Modifications were detected since last access.", duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.Show();
        }

        public static void NotAuthorizedToast(int duration = -1)
        {
            var toastNotification = new Notification("Not Authorized", @"You are not authorized to change this value", duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.Show();
        }

        public static void NoChangesOnTemplateToast(int duration = 2)
        {
           var toastNotification = new Notification("Change not allowed", "Reverting change", duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
           toastNotification.Show();
        }

        public static void NoChangesOnDerivedValuesToast(int duration = 2)
        {
            var toastNotification = new Notification("Change not allowed", "Value is a calculated value", duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.Show();
        }

        public static void DerivationErrors(ResponseDerivationError[] errors, int duration = -1)
        {
            var sb = new StringBuilder();
            foreach (var error in errors)
            {
                var toastNotification = new Notification("Correct errors and retry", error.M, duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                toastNotification.Show();
            }
        }

        public static void GeneralError(Response error, int duration = -1)
        {
            var toastNotification = new Notification("General Error", error.ErrorMessage, duration, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
            toastNotification.Show();
        }
    }
}
