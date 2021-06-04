namespace OV.MainDb.UserElection.Models.Public
{
    public class CandidateUserElection
    {
        public int? Id { get; set; }
        public int TblUser_UID { get; set; }
        public int TblElection_UID { get; set; }

        public CandidateUserElection(int tblUser_UID, int tbl_Election_UID)
        {
            TblUser_UID = tblUser_UID;
            TblElection_UID = tbl_Election_UID;
        }

        public CandidateUserElection()
        {
        }
    }
}
