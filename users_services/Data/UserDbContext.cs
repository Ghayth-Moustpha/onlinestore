﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UsersService.Models;

namespace UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}
