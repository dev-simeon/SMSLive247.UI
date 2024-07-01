namespace SMSLive247.UI.Pages.Messaging.Compose
{
    /// <summary>
    /// Represents a contact model used in the messaging compose functionality.
    /// </summary>
    public record class ContactModel
    {
        /// <summary>
        /// Gets the unique key of the contact.
        /// </summary>
        public string Key { get; init; }

        /// <summary>
        /// Gets the name of the contact.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the count associated with the contact (e.g., number of members in a group).
        /// </summary>
        public int Count { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the contact is selected.
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the contact is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactModel"/> class.
        /// </summary>
        /// <param name="key">The unique key of the contact.</param>
        /// <param name="name">The name of the contact.</param>
        /// <param name="count">The count associated with the contact.</param>
        /// <exception cref="ArgumentNullException">Thrown when the key or name is null or empty.</exception>
        public ContactModel(string key, string name, int count)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
            }

            Key = key;
            Name = name;
            Count = count;
        }

        public override string ToString() => $"{Name} ({Count})";
    }
}
