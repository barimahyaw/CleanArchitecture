namespace Domain.Shared;

public static class DomainErrors
{
    public static class Gathering
    {
        public static string Expired => "Invitation is expired.";
    }

    public static class Invitation
    {
        public static string Invalid => "Invitation is invalid.";
    }
}
