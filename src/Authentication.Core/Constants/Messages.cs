namespace Authentication.Core.Constants
{
    public static class SuccessfulMessages
    {
        public static string UserRegister = "Usuario registrado exitosamente.";
        public static string UserNotFound = "El usuario o la contraseña son incorrectos.";
        public static string UserSendConfirmationEmail = "Correo de confirmacion enviado exitosamente.";
        public static string UserSendResetPassword = "Correo de recuperacion de contraseña enviado exitosamente.";
        public static string UserConfirmationEmail = "Correo confirmado exitosamente.";
        public static string PasswordUpdate = "Contraseña actualizada exitosamente.";
        public static string UserUpdate = "La informacion ha sido actualizada exitosamente.";
        public static string UserDelete = "El usuario ha sido borrado exitosamente.";

    }

    public static class FailedMessages
    {
        public static string UserRegister = "Hubo un error al registrar al usuario.";
        public static string UserNotFound = "El usuario o la contraseña son incorrectos.";
        public static string UserBlocked = "Usuario bloqueado.";
        public static string UserNotConfirmed = "Telefono o correo electronico si confirmar.";
        public static string PasswordIncorrect = "El usuario o la contraseña son incorrectos.";
        public static string TokenIncorrect = "Token no valido.";
        public static string Error = "Hubo un problema.";
        public static string UserSendConfirmationEmail = "Hubo un problema al tratar de enviar el correo";
        public static string UserSendResetPassword = "Hubo un problema al tratar de enviar el correo.";
        public static string UserConfirmationEmail = "No se pudo confirmar el correo.";
        public static string PasswordUpdate = "Hubo un problema al actualizar la contraseña";
        public static string UserUpdate = "Hubo un problema al tratar de actualizar la informacion.";
        public static string UserDelete = "Hubo un problema eliminando al usuario.";

    }
}
