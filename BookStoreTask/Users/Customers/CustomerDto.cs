﻿using BookStoreTask.Users.BaseUser.Dto;

namespace BookStoreTask.Users.Customers;

public class CustomerDto:UserDto
{
    public CustomerStatus CustomerStatus { get; set; }
    public Guid? CartId { get; set; }
}