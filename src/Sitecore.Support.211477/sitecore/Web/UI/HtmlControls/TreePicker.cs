namespace Sitecore.Support.Web.UI.HtmlControls
{
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Sheer;
  using System;
  using System.Web.UI;
  using System.Web.UI.WebControls;

  /// <summary>
  /// Represents a combobox with treeview dropdown.
  /// </summary>
  public class TreePicker : Sitecore.Web.UI.HtmlControls.TreePicker
  {
    /// <summary>
    /// Called when the dropdown glyph is clicked.
    /// </summary>
    protected override void DropDown()
    {
      if (!string.IsNullOrEmpty(this.Value))
      {
        DataContext dataContext = Sitecore.Context.ClientPage.FindSubControl(this.DataContext) as DataContext;
        Assert.IsNotNull(dataContext, typeof(DataContext), "Datacontext \"{0}\" not found.", this.DataContext);
        dataContext.Folder = this.Value;
      }
      System.Web.UI.Control hiddenHolder = UIUtil.GetHiddenHolder(this);
      DataTreeNode dataTreeNode = null;
      Scrollbox scrollbox = new Scrollbox();
      Sitecore.Context.ClientPage.AddControl(hiddenHolder, scrollbox);
      scrollbox.Width = 300;
      scrollbox.Height = 400;
      DataTreeview dataTreeview = new DataTreeview();
      dataTreeview.Class = "scTreeview scPopupTree";
      dataTreeview.DataContext = this.DataContext;
      dataTreeview.ID = this.ID + "_treeview";
      dataTreeview.AllowDragging = false;
      if (this.AllowNone)
      {
        dataTreeNode = new DataTreeNode();
        Sitecore.Context.ClientPage.AddControl(dataTreeview, dataTreeNode);
        dataTreeNode.ID = this.ID + "_none";
        dataTreeNode.Header = Translate.Text("[none]");
        dataTreeNode.Expandable = false;
        dataTreeNode.Expanded = false;
        dataTreeNode.Value = "none";
        dataTreeNode.Icon = "Applications/16x16/forbidden.png";
      }
      Sitecore.Context.ClientPage.AddControl(scrollbox, dataTreeview);
      dataTreeview.Width = new Unit(100.0, UnitType.Percentage);
      dataTreeview.Click = this.ID + ".Select";
      dataTreeview.DataContext = this.DataContext;
      if (string.IsNullOrEmpty(this.Value) && dataTreeNode != null)
      {
        dataTreeview.ClearSelection();
        dataTreeNode.Selected = true;
      }
      SheerResponse.ShowPopup(this.ID, "below-right", scrollbox);
    }
  }
}