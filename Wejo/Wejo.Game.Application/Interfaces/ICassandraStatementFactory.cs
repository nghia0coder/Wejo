﻿using Cassandra;

namespace Wejo.Game.Application.Interfaces;

public interface ICassandraStatementFactory
{
    PreparedStatement CreateInsertMessageStatement();
    PreparedStatement CreateInsertMessageByUserStatement();
    PreparedStatement CreateSelectMessagesBeforeStatement();
    PreparedStatement CreateSelectMessagesAfterStatement();
    PreparedStatement CreateSelectMessagesBeforeFromUserStatement();
    PreparedStatement CreateSelectMessagesAfterFromUserStatement();
    PreparedStatement CreateSelectReadStatusStatement();
    PreparedStatement CreateUpdateReadStatusStatement();
}
