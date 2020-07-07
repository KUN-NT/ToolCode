using System.Windows.Controls;

namespace PrintDemo
{
    public static class ComboBoxExtension
    {
        public static void LocationComboBoxByText(this ComboBox cb, string itemText)
        {
            cb.SelectedIndex = -1;

            if (string.IsNullOrWhiteSpace(itemText))
            {
                return;
            }

            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (cb.Items[i].ToString().Trim().ToUpper() == itemText.Trim().ToUpper())
                {
                    cb.SelectedIndex = i;
                }
            }
        }

    }
}
