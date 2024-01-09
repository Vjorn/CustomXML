using System.Text;
using CustomXMLSerializer.Core.Attributes.Helpers;
using CustomXMLSerializer.Core.Helpers;
using CustomXMLSerializer.Models.Parts.Events;
using CustomXMLSerializer.Models.Parts.Events.Enums;

namespace CustomXMLSerializer.Data.Builders;

public class RecordsBuilder
{
    private readonly int _currentLength;
    private readonly int _maxLength;
    
    public RecordsBuilder(int currentLength, int maxLength)
    {
        _currentLength = currentLength;
        _maxLength = maxLength;
    }
    
    public string GetRecordString(EventType eventType, int recordNumber)
    {
        switch (eventType)
        {
            case EventType.SubjectMainChanged:
                return GetSubjectMainChangedString(recordNumber);
            
            case EventType.LeasingDealMade:
            case EventType.SubjectSpecialChanged:
            case EventType.SubjectCapacityChanged:
            case EventType.MonetaryDealChanged:
            case EventType.MonetaryDealFunded:
            case EventType.MonetaryDealPerformanceChanged:
            case EventType.DealSecuringChanged:
            case EventType.MonetaryDealEnded:
            case EventType.DealClaimChanged:
            case EventType.DealInfoStopped:
            case EventType.CreditorChanged:
            case EventType.ServiceOrgChanged:
            case EventType.MonetaryDealAcquired:
            case EventType.SubjectBlocksCorrected:
            case EventType.DealCorrected:
            case EventType.DealRemoved:
            case EventType.ApplicationRemoved:
            case EventType.SubjectRemoved:
            case EventType.BlocksAnnulled:
            case EventType.DealAnnulled:
            case EventType.SubjectAnnulled:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        }

        return "";
    }

    private string GetSubjectMainChangedString(int recordNumber)
    {
        SubjectMainChangedEvent eventModel = new SubjectMainChangedEvent();
        
        Type eventTypeType = eventModel.GetType();
        ModelInfoCollector eventModelInfoCollector = new ModelInfoCollector(eventTypeType);
        
        StringBuilder stringBuilder = new StringBuilder();
        
        string recordNumberAttributeKey = $"{eventTypeType.Name}.{nameof(eventModel.RecordNumber)}";
        string eventAttributeKey = $"{eventTypeType.Name}.{nameof(eventModel.Event)}";
        string eventDateAttributeKey = $"{eventTypeType.Name}.{nameof(eventModel.EventDate)}";
        string actionAttributeKey = $"{eventTypeType.Name}.{nameof(eventModel.Action)}";
        
        stringBuilder.Append(
            XmlStringBuilder.WriteStartElement(eventModelInfoCollector.GetRootElementInfo().Name, true));
        stringBuilder.Append(XmlStringBuilder.WriteAttributeString(eventModelInfoCollector.GetAttributeByKey(recordNumberAttributeKey).Name,
            recordNumber.ToString()));
        stringBuilder.Append(XmlStringBuilder.WriteAttributeString(eventModelInfoCollector.GetAttributeByKey(eventAttributeKey).Name,
            "1.7"));
        stringBuilder.Append(XmlStringBuilder.WriteAttributeString(eventModelInfoCollector.GetAttributeByKey(eventDateAttributeKey).Name,
            DateTime.Today.ToShortDateString()));
        stringBuilder.Append(XmlStringBuilder.WriteAttributeString(eventModelInfoCollector.GetAttributeByKey(actionAttributeKey).Name,
            "B"));
        stringBuilder.Append(">");
        
        stringBuilder.Append(TitlePartBuilder.GetTitlePartString());

        stringBuilder.Append(
            XmlStringBuilder.WriteEndElement(eventModelInfoCollector.GetRootElementInfo().Name));
        
        return stringBuilder.ToString();
    }
}