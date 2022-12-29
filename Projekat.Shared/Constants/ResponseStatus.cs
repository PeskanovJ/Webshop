namespace Projekat.Shared.Constants
{
    public enum ResponseStatus
    {
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500,
        InvalidEmail = 1000,
        InvalidPhoneNo = 1001,
        AccountNotActivated=1002,
        InvalidPasswordGuid=1003,
        CategoryNotFound=1004,
        AccountAlreadyActivated=1005,
    }
}
