namespace OV.MainDb.Option
{
    public enum OptionFailureReason
    {
        IdIsEmpty,
        NameIsEmpty,
        DescriptionIsEmpty,
        TblElection_UIDIsEmpty,
        TblUser_UIDIsEmpty,
        TblOption_UIDIsEmpty,
        RelationUserElectionAlreadyExists,
        FailureInsertingIntoDataBase,
        FailureIncreasingVotes
    }
}
