namespace Sitecore.Support.Web.UI.HtmlControls
{
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Web.UI.HtmlControls;

  /// <summary>Displays hierarchical data, such as a table of contents, in a tree structure.</summary>
  public class DataTreeview : Sitecore.Web.UI.HtmlControls.DataTreeview
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Sitecore.Web.UI.HtmlControls.DataTreeview" /> class.
    /// </summary>
    public DataTreeview() : base() { }

    /// <summary>Gets the tree node.</summary>
    /// <param name="item">The item.</param>
    /// <param name="parent">The parent.</param>
    /// <returns>The tree node.</returns>
    protected override TreeNode GetTreeNode(Item item, System.Web.UI.Control parent)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(parent, "parent");
      DataTreeNode dataTreeNode = new DataTreeNode();
      parent.Controls.Add(dataTreeNode);
      dataTreeNode.Expandable = item.HasChildren;
      dataTreeNode.Expanded = false;
      #region Modified code
      dataTreeNode.Header = ItemExtension.GetUIDisplayName(item);
      #endregion
      dataTreeNode.Icon = item.Appearance.Icon;
      dataTreeNode.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("T");
      dataTreeNode.ItemID = item.Paths.LongID;
      dataTreeNode.ToolTip = item.Name;
      dataTreeNode.DataContext = this.DataContext;
      if (item.TemplateID != TemplateIDs.TemplateField)
      {
        dataTreeNode.ItemStyle = item.Appearance.Style;
      }
      if (dataTreeNode.ItemStyle.Length == 0)
      {
        if (item.Appearance.Hidden)
        {
          dataTreeNode.ItemStyle = "color:#666666";
        }
        else if (item.RuntimeSettings.IsVirtual)
        {
          dataTreeNode.ItemStyle = "color:#666666";
        }
      }
      dataTreeNode.ShowCheckBox = base.ShowCheckboxes;
      if (this.AllowDragging)
      {
        dataTreeNode.Drag = "item:" + item.Uri;
        dataTreeNode.Drop = this.ID + ".DropItem(\"$Data\")";
      }
      foreach (string key in base.ColumnNames.Keys)
      {
        string text2 = ((BaseItem)item)[key];
        if (text2.Length > 0)
        {
          dataTreeNode.ColumnValues[key] = text2;
        }
      }
      return dataTreeNode;
    }
  }
}