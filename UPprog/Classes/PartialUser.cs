namespace UPprog
{
    public partial class User
    {
        public string FIO => UserSurname + " " + UserName + " " + UserPatronymic;
    }
}
