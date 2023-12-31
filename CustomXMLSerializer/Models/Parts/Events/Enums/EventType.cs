namespace CustomXMLSerializer.Models.Parts.Events.Enums;

/// <summary>
/// Типы событий кредитной истории
/// </summary>
public enum EventType
{
    /// <summary>
    /// 1.4.1 Субъект и источник заключили договор лизинга либо поручительства по лизингу и предмет лизинга передан лизингополучателю
    /// </summary>
    LeasingDealMade = 1,
    /// <summary>
    /// 1.7 Изменились сведения титульной части кредитной истории субъекта
    /// <para></para>
    /// Триггер:
    /// Если поменялось ФИО и номер/дата выдачи паспорта одновременно, значит человек сменил фамилию.
    /// </summary>
    SubjectMainChanged,
    /// <summary>
    /// 1.9 Изменились сведения о субъекте в основной части кредитной истории, кроме сведений о дееспособности, банкротстве, индивидуальном рейтинге и кредитной оценке
    /// <para></para>
    /// Триггер:
    /// Поменялся Адрес регистрации или контактные данные
    /// </summary>
    SubjectSpecialChanged,
    /// <summary>
    /// 1.10 Изменились сведения о дееспособности субъекта
    /// </summary>
    SubjectCapacityChanged,
    /// <summary>
    /// 2.1 Изменились сведения об условиях обязательства субъекта для денежного обязательства
    /// </summary>
    MonetaryDealChanged,
    /// <summary>
    /// 2.2 Субъекту передана сумма займа (кредита) для денежного обязательства
    /// </summary>
    MonetaryDealFunded,
    /// <summary>
    /// 2.3 Изменились сведения об исполнении обязательства субъектом, наступила ответственность поручителя или обязательство принципала возместить выплаченную сумму для денежного обязательства
    /// <para></para>
    /// Триггер:
    /// Появились НОВЫЕ платежи с датой позже последнего имеющегося
    /// </summary>
    MonetaryDealPerformanceChanged,
    /// <summary>
    /// 2.4 Изменились сведения об обеспечении исполнения обязательства
    /// </summary>
    DealSecuringChanged,
    /// <summary>
    /// 2.5 Обязательство субъекта прекратилось для денежного обязательства
    /// <para></para>
    /// Триггер:
    /// Проставлен закрывающий статус в DBA, например "списание" или "закрыт" итд. Проставлена дата банкротства или дата закрытия 
    /// </summary>
    MonetaryDealEnded,
    /// <summary>
    /// 2.6 Изменились сведения о судебном споре или требовании по обязательству
    /// <para></para>
    /// Триггер:
    /// Изменились любые данные, связанные с судебными решениями (банкротво, взыскание итд)
    /// </summary>
    DealClaimChanged,
    /// <summary>
    /// 2.10 Источник прекратил передачу информации по обязательству
    /// </summary>
    DealInfoStopped,
    /// <summary>
    /// 2.11 Права кредитора по обязательству перешли к другому лицу
    /// Триггер:
    /// где лист == возврат цеденту
    /// </summary>
    CreditorChanged,
    /// <summary>
    /// 2.12 Изменились сведения об обслуживающей организации в частности, заключен, изменен или расторгнут договор обслуживания
    /// </summary>
    ServiceOrgChanged,
    /// <summary>
    /// 1.4 Субъект и источник совершили сделку, кроме договора лизинга и поручительства по лизингу
    /// <para/>
    /// Сейчас используется только для удаления
    /// </summary>
    [Obsolete("Use " + nameof(DealRemoved))]
    MonetaryDealMade,
    /// <summary>
    /// 2.11.2 Права кредитора по обязательству получены от другого лица для денежного обязательства
    /// <para/>
    /// Новое событие, добавляется автоматически всем новым SourceModel
    /// </summary>
    MonetaryDealAcquired,
    /// <summary>
    /// 3.1 Требуется внесение изменений в связи с ошибкой в показателях кредитной истории, не связанных с записью кредитной истории
    /// <para/>
    /// Триггер:
    /// Статус <see cref="SourceModel.DatabaseComparisonResult"/> = <see cref="DatabaseComparisonResult.Conflict"/>
    /// И/ИЛИ Опечатка в данных субьекта:
    /// <list type="bullet">
    /// <item>ФИО изменилось, но номер паспорта и дата его выдачи осталась прежней.</item>
    /// <item>Номер паспорта поменялся, но дата выдачи осталась прежней</item>
    /// <item>Дата выдачи паспорта поменялась, номер - нет</item>
    /// <item>место рождения поменялось</item>
    /// </list>
    /// </summary>
    SubjectBlocksCorrected,
    /// <summary>
    /// 3.2 Требуется внесение изменений в связи с ошибкой в показателях по событиям, связанным с записью кредитной истории
    /// <para/>
    /// Триггер:
    /// Добавились или удалились платежи за даты РАНЬШЕ последнего имеющегося
    /// </summary>
    DealCorrected,
    /// <summary>
    /// 3.3 Требуется исключение записи кредитной истории, сведения о которой отсутствуют у источника формирования кредитной истории
    /// </summary>
    DealRemoved,
    /// <summary>
    /// 3.4 Требуется исключение сведений об обращении, сведения о котором отсутствуют у источника формирования кредитной истории 
    /// </summary>
    ApplicationRemoved,
    /// <summary>
    /// 3.5 Требуется исключение кредитной истории субъекта, сведения о которой отсутствуют у источника формирования кредитной истории
    /// </summary>
    SubjectRemoved,
    /// <summary>
    /// 4.1 Требуется аннулирование отдельных показателей кредитной истории (subjectBlocksAnnulled) 
    /// </summary>
    BlocksAnnulled,
    /// <summary>
    /// 4.2 Требуется аннулирование записи кредитной истории (dealAnnulled)
    /// </summary>
    DealAnnulled,
    /// <summary>
    /// 4.3 Требуется аннулирование всей кредитной истории (subjectAnnulled)
    /// </summary>
    SubjectAnnulled
}