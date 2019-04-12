using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Analytics;
using Sitecore.Analytics.Outcome.Model;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FXM.Pipelines.Tracking.RegisterPageEvent;
using Sitecore.FXM.Utilities;

namespace Sitecore.Support.FXM.Pipelines.Tracking.RegisterPageEvent
{
  public class RegisterPageEventProcessor : Sitecore.FXM.Pipelines.Tracking.RegisterPageEvent.RegisterPageEventProcessor
  {
    protected override void TriggerOutcome(RegisterPageEventArgs args)
    {
      if (Tracker.Current != null)
      {
        ID newID = ID.NewID;
        ID interactionId = ID.Parse(Tracker.Current.Interaction.InteractionId);
        ID contactId = ID.Parse(Tracker.Current.Contact.ContactId);
        Item pageEventItem = args.PageEventItem;
        ContactOutcome contactOutcome = new ContactOutcome(newID, pageEventItem.ID, contactId)
        {
          DateTime = DateTime.UtcNow.Date,
          InteractionId = interactionId,
          MonetaryValue = args.EventParameters.MonetaryValue
        };
        foreach (KeyValuePair<string, string> extra in args.EventParameters.Extras)
        {
          contactOutcome.CustomValues[extra.Key] = extra.Value;
        }

        OutcomeUtility.TriggerOutcome(contactOutcome);
      }
    }
  }
}