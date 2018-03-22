namespace Sitecore.Support.Shell.Applications.ContentEditor.FieldHelpers
{
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Applications.ContentEditor;
  using System.Text;

  /// <summary>
  ///   Composes a filter for <see cref="T:Sitecore.Shell.Applications.ContentEditor.TreeList" /> that limits the items to be shown.
  ///   <para>
  ///     Builds a Sitecore XPath query that will filter candidates via <see cref="T:Sitecore.Data.Query.Predicate" />
  ///     instance.
  ///   </para>
  /// </summary>
  public class TreeListFilterQueryBuilder
  {
    /// <summary>
    ///   Gets the item condition for filtering items either by ID, or Key.
    /// </summary>
    /// <value>
    ///   The item condition.
    /// </value>
    public virtual string ItemCondition => "(contains(',{0},', ',' + @@id + ',') or contains(',{0},', ',' + @@key + ','))";

    /// <summary>
    ///   Gets the template condition for filtering items either by Template key, or ID.
    /// </summary>
    /// <value>
    ///   The template condition.
    /// </value>
    public virtual string TemplateCondition => "(containscaseinsensitive(',{0},', ',' + @@templateid + ',') or contains(',{0},', ',' + @@templatekey + ','))";

    /// <summary>
    ///   <para>
    ///     Builds a Sitecore XPath query that will filter candidates via <see cref="T:Sitecore.Data.Query.Predicate" />
    ///     instance.
    ///   </para>
    /// </summary>
    /// <param name="treeList">The tree list.</param>
    /// <returns><c>string.Empty</c> in case all conditions are empty. Sitecore xPath query for filtering of visible items.</returns>
    public virtual string BuildFilterQuery(Sitecore.Support.Shell.Applications.ContentEditor.TreeList treeList)
    {
      Assert.ArgumentNotNull(treeList, "treeList");
      return this.BuildFilterQuery(treeList.IncludeTemplatesForDisplay, treeList.ExcludeTemplatesForDisplay, treeList.IncludeItemsForDisplay, treeList.ExcludeItemsForDisplay);
    }

    /// <summary>
    ///   <para>
    ///     Builds a Sitecore XPath query that will filter candidates via <see cref="T:Sitecore.Data.Query.Predicate" />
    ///     instance.
    ///   </para>
    /// </summary>
    /// <param name="includeTemplatesForDisplay">The include templates for display. Comma separated IDs, and names.</param>
    /// <param name="excludeTemplatesForDisplay">The exclude templates for display. Comma separated IDs, and names.</param>
    /// <param name="includeItemsForDisplay">The include items for display.</param>
    /// <param name="excludeItemsForDisplay">The exclude items for display.</param>
    /// <returns><c>string.Empty</c> in case all conditions are empty. Sitecore xPath query for filtering of visible items.</returns>
    public virtual string BuildFilterQuery(string includeTemplatesForDisplay = null, string excludeTemplatesForDisplay = null, string includeItemsForDisplay = null, string excludeItemsForDisplay = null)
    {
      if (string.IsNullOrEmpty(includeTemplatesForDisplay) && string.IsNullOrEmpty(excludeTemplatesForDisplay) && string.IsNullOrEmpty(includeItemsForDisplay) && string.IsNullOrEmpty(excludeItemsForDisplay))
      {
        return string.Empty;
      }
      StringBuilder stringBuilder = new StringBuilder();
      this.AppendCondition(stringBuilder, includeTemplatesForDisplay, this.TemplateCondition, false);
      this.AppendCondition(stringBuilder, excludeTemplatesForDisplay, this.TemplateCondition, true);
      this.AppendCondition(stringBuilder, includeItemsForDisplay, this.ItemCondition, false);
      this.AppendCondition(stringBuilder, excludeItemsForDisplay, this.ItemCondition, true);
      return stringBuilder.ToString();
    }

    /// <summary>
    ///   Appends the <paramref name="xPathCondition" /> in case <paramref name="conditionValues" /> is not empty.
    ///   <para>Adds 'and' statement before appending new condition if needed.</para>
    /// </summary>
    /// <param name="queryBuilder">The resulting query statement that carries all the conditions.</param>
    /// <param name="conditionValues">
    ///   The values would be converted to lower and appended into <paramref name="queryBuilder" />
    ///   .
    /// </param>
    /// <param name="xPathCondition">The xPath condition.</param>
    /// <param name="negativeStatement">if set to <c>true</c> 'not' statement will be appended before the condition.</param>    
    protected virtual void AppendCondition(StringBuilder queryBuilder, string conditionValues, string xPathCondition, bool negativeStatement)
    {
      Assert.ArgumentNotNull(queryBuilder, "queryBuilder");
      if (!string.IsNullOrEmpty(conditionValues))
      {
        if (queryBuilder.Length != 0)
        {
          queryBuilder.Append(" and ");
        }
        if (negativeStatement)
        {
          queryBuilder.Append("not ");
        }
        queryBuilder.AppendFormat(xPathCondition, conditionValues.ToLowerInvariant());
      }
    }
  }
}