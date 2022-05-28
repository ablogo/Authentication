namespace Authentication.Core.Dtos
{
    public class ServiceResult
    {
        public string Message { get; set; }

        public bool Succeeded { get; set; } = false;
    }

    public class ServiceResult<T>
    {
        public string Message { get; set; }

        public T Object { get; set; }

        public bool Succeeded { get; set; } = false;
    }
}
