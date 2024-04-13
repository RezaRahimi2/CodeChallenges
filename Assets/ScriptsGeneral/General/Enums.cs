namespace Immersed.General
{
    public enum RequestName
    {
        None,
        AnonymousLogin,
        Register,
        EmailLogin,
        UpdateRegisteredUserData,
        UpdateGuestUserData,
        GetDataWithAuthToken,
        Purchase
    }

    public enum UserClass
    {
        Guest,
        Registered
    }

    public enum APIVersion
    {
        None,
        V1,
        V2
    }
}