﻿namespace Wejo.Common.SeedWork.Dtos;

public abstract class MessageDto
{
    public Guid MessageId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedOn { get; set; }
}

