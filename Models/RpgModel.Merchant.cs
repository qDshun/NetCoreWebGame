﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 23.05.2019 15:54:56
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
    public partial class Merchant
    {

        public Merchant()
        {
            OnCreated();
        }

        public virtual int Gold
        {
            get;
            set;
        }

        public virtual int? MealWorth
        {
            get;
            set;
        }

        public virtual int MerchantId
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual int? RoomRentWorth
        {
            get;
            set;
        }

        public virtual string ShopName
        {
            get;
            set;
        }

        public virtual string Type
        {
            get;
            set;
        }

        public virtual int XPos
        {
            get;
            set;
        }

        public virtual int YPos
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
