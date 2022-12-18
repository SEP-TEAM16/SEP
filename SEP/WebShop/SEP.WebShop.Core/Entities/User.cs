using SEP.WebShop.Core.Entities.ValueObjects;

namespace SEP.WebShop.Core.Entities
{
    public abstract class User
    {
        public Guid Id { get; }
        public Username Username { get; }
        public HashedPassword Password { get; }
        public EmailAddress EmailAddress { get; }

        protected User(Guid id, Username username, HashedPassword password, EmailAddress email)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.EmailAddress = email;
        }

        protected User(Guid id, Username username, EmailAddress email)
        {
            this.Id = id;
            this.Username = username;
            this.EmailAddress = email;
        }

    }
}
