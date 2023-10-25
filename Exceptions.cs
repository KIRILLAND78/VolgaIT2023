namespace VolgaIT2023
{
    public class UnauthorizedException: Exception
    {
        public override string Message => "Unauthorized";
    }
    public class ForbiddenException : Exception
    {
        public override string Message => "Forbidden";
    }
    public class NotFoundException : Exception
    {
        private string type = "";
        public NotFoundException(string t) {type = t; }
        public override string Message => $"{type} Not Found";
    }
}
