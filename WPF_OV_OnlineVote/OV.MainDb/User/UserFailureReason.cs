namespace OV.MainDb.User
{
    public enum UserFailureReason
    {
        FirstNameIsEmpty,
        FirstSurNameIsEmpty,
        PasswordIsEmpty,
        DateOfBirthIsEmpty,
        AutonomousCommunityIsEmpty,
        AutonomousCommunityDoesNotExist,
        ProvinceIsEmpty,
        ProvinceDoesNotExist,
        EmailIsEmpty,
        DNI_NIEAlreadyExist,
        PhoneNumberIsEmpty,
        FailureInsertingIntoDataBase
    }
}
