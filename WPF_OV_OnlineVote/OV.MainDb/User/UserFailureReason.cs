using System;
using System.Collections.Generic;
using System.Text;

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
        PhoneNumberIsEmpty,
        FailureInsertingIntoDataBase
    }
}
