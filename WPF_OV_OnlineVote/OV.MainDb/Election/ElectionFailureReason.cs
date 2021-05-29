namespace OV.MainDb.Election
{
    public enum ElectionFailureReason
    {
        NameIsEmpty,
        InitDateIsEmpty,
        FinishDateIsEmpty,
        AutonomousCommunityIsEmpty,
        AutonomousCommunityDoesNotExist,
        ProvinceIsEmpty,
        ProvinceDoesNotExist,
        TypeIsEmpty,
        TypeDoesNotExist,
        Description,
        FailureInsertingIntoDataBase
    }
}
