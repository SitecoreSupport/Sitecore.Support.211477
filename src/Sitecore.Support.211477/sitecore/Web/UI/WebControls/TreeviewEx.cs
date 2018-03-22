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
      string text = string.IsNullOrEmpty(this.DisplayFieldName) ? item.DisplayName : ((BaseItem)item)[this.DisplayFieldName];
      return string.IsNullOrEmpty(text) ? item.DisplayName : text;
    }
  }
}