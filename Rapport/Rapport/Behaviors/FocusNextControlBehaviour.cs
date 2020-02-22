using Xamarin.Forms;

namespace Rapport.Behaviors
{
    public class FocusNextEntryBehavior : Behavior<Entry>
    {
        public Entry NextEntry { get; set; }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.Completed += Entry_Completed;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.Completed -= Entry_Completed;
            base.OnDetachingFrom(entry);
        }

        private void Entry_Completed(object sender, System.EventArgs e)
        {
            NextEntry?.Focus();
        }
    }
}
