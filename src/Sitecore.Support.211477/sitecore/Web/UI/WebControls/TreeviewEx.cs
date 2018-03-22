namespace Sitecore.Support.Web.UI.WebControls
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>Light-weight treeview control.</summary>
  public class TreeviewEx : Sitecore.Web.UI.WebControls.TreeviewEx
  {
    /// <summary>
    /// Gets the header value.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>Header text for list item.</returns>
    protected override string GetHeaderValue(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      #region Modified code
      string text = string.IsNullOrEmpty(this.DisplayFieldName) ? ItemExtension.GetUIDisplayName(item) : ((BaseItem)item)[this.DisplayFieldName];
      #endregion
      return string.IsNullOrEmpty(text) ? item.DisplayName : text;
    }
  }
}