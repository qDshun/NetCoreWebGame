﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 19.05.2019 13:05:02
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;

namespace gameModel
{
    public partial class User {

        public User()
        {
            this.Characters = new List<Character>();
            OnCreated();
        }

        public virtual int UserId
        {
            get;
            set;
        }

        public virtual string Email
        {
            get;
            set;
        }

        public virtual string Login
        {
            get;
            set;
        }

        public virtual string PasswordHash
        {
            get;
            set;
        }

        public virtual IList<Character> Characters
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
