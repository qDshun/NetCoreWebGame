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
    public partial class Zone {

        public Zone()
        {
            OnCreated();
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

        public virtual int Type
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Zone toCompare = obj as Zone;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.XPos, toCompare.XPos))
            return false;
          if (!Object.Equals(this.YPos, toCompare.YPos))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + XPos.GetHashCode();
          hashCode = (hashCode * 7) + YPos.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
