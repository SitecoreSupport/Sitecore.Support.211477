namespace Sitecore.Support.Shell.Applications.ContentEditor.Dialogs.TreeListExEditor
{
  using Sitecore;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Applications.ContentEditor;
  using Sitecore.Web;
  using Sitecore.Web.UI.Pages;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.XmlControls;
  using System;

  /// <summary>
  /// Represents the TreeListEx Editor form.
  /// </summary>
  public class TreeListExEditorForm : DialogForm
  {
    /// <summary></summary>
    protected XmlControl Dialog;

    /// <summary></summary>
    protected TreeList TreeList;

    /// <summary>
    /// Raises the load event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
    /// <remarks>
    /// This method notifies the server control that it should perform actions common to each HTTP
    /// request for the page it is associated with, such as setting up a database query. At this
    /// stage in the page lifecycle, server controls in the hierarchy are created and initialized,
    /// view state is restored, and form controls reflect client-side data. Use the IsPostBack
    /// property to determine whether the page is being loaded in response to a client postback,
    /// or if it is being loaded and accessed for the first time.
    /// </remarks>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (!Context.ClientPage.IsEvent)
      {
        UrlHandle urlHandle = UrlHandle.Get();
        this.TreeList.Source = urlHandle["source"];
        this.TreeList.SetValue(StringUtil.GetString(urlHandle["value"]));
        this.TreeList.ItemLanguage = urlHandle["language"];
        this.TreeList.ItemID = urlHandle["itemID"];
        if (!string.IsNullOrEmpty(urlHandle["title"]))
        {
          this.Dialog["Header"] = urlHandle["title"];
        }
        if (!string.IsNullOrEmpty(urlHandle["text"]))
        {
          this.Dialog["text"] = urlHandle["text"];
        }
        if (!string.IsNullOrEmpty(urlHandle["icon"]))
        {
          this.Dialog["icon"] = urlHandle["icon"];
        }
      }
    }

    /// <summary>
    /// Handles a click on the OK button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <remarks>When the user clicks OK, the dialog is closed by calling
    /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.</remarks>
    protected override void OnOK(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");
      string text = this.TreeList.GetValue();
      if (text.Length == 0)
      {
        text = "-";
      }
      SheerResponse.SetDialogValue(text);
      base.OnOK(sender, args);
    }
  }
}