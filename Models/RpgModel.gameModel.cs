﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 19.05.2019 13:05:01
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace gameModel
{

    public partial class gameModel : DbContext
    {

        public gameModel() :
            base()
        {
            OnCreated();
        }

        public gameModel(DbContextOptions<gameModel> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
                optionsBuilder.UseMySQL(@"persistsecurityinfo=True;server=localhost;user id=root;password=Kappa2012;database=game");
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Character> Characters
        {
            get;
            set;
        }

        public virtual DbSet<CharacterInfo> CharacterInfos
        {
            get;
            set;
        }

        public virtual DbSet<CharacterState> CharacterStates
        {
            get;
            set;
        }

        public virtual DbSet<ExistingItem> ExistingItems
        {
            get;
            set;
        }

        public virtual DbSet<ExistingMonster> ExistingMonsters
        {
            get;
            set;
        }

        public virtual DbSet<Item> Items
        {
            get;
            set;
        }

        public virtual DbSet<ItemType> ItemTypes
        {
            get;
            set;
        }

        public virtual DbSet<Monster> Monsters
        {
            get;
            set;
        }

        public virtual DbSet<Merchant> Merchants
        {
            get;
            set;
        }

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Zone> Zones
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.CharacterMapping(modelBuilder);
            this.CustomizeCharacterMapping(modelBuilder);

            this.CharacterInfoMapping(modelBuilder);
            this.CustomizeCharacterInfoMapping(modelBuilder);

            this.CharacterStateMapping(modelBuilder);
            this.CustomizeCharacterStateMapping(modelBuilder);

            this.ExistingItemMapping(modelBuilder);
            this.CustomizeExistingItemMapping(modelBuilder);

            this.ExistingMonsterMapping(modelBuilder);
            this.CustomizeExistingMonsterMapping(modelBuilder);

            this.ItemMapping(modelBuilder);
            this.CustomizeItemMapping(modelBuilder);

            this.ItemTypeMapping(modelBuilder);
            this.CustomizeItemTypeMapping(modelBuilder);

            this.MonsterMapping(modelBuilder);
            this.CustomizeMonsterMapping(modelBuilder);

            this.MerchantMapping(modelBuilder);
            this.CustomizeMerchantMapping(modelBuilder);

            this.UserMapping(modelBuilder);
            this.CustomizeUserMapping(modelBuilder);

            this.ZoneMapping(modelBuilder);
            this.CustomizeZoneMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region Character Mapping

        private void CharacterMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().ToTable(@"character", @"game");
            modelBuilder.Entity<Character>().Property<int>(x => x.CharacterId).HasColumnName(@"character_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Character>().Property<string>(x => x.Nickname).HasColumnName(@"nickname").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<Character>().Property<System.DateTime>(x => x.Birthdate).HasColumnName(@"birthdate").HasColumnType(@"datetime").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Character>().Property<int>(x => x.UserId).HasColumnName(@"user_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Character>().HasKey(@"CharacterId");
            modelBuilder.Entity<Character>().HasIndex(@"Nickname").IsUnique(true);
        }

        partial void CustomizeCharacterMapping(ModelBuilder modelBuilder);

        #endregion

        #region CharacterInfo Mapping

        private void CharacterInfoMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterInfo>().ToTable(@"character_info", @"game");
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.CharacterId).HasColumnName(@"character_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<string>(x => x.Race).HasColumnName(@"race").HasColumnType(@"varchar").ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<CharacterInfo>().Property<string>(x => x.Sex).HasColumnName(@"sex").HasColumnType(@"enum").ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int?>(x => x.Level).HasColumnName(@"level").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Str).HasColumnName(@"str").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Dex).HasColumnName(@"dex").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Int).HasColumnName(@"int").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Con).HasColumnName(@"con").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Cha).HasColumnName(@"cha").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Exp).HasColumnName(@"exp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().Property<int>(x => x.Gold).HasColumnName(@"gold").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"0");
            modelBuilder.Entity<CharacterInfo>().Property<int?>(x => x.StatPoints).HasColumnName(@"stat_points").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterInfo>().HasKey(@"CharacterId");
        }

        partial void CustomizeCharacterInfoMapping(ModelBuilder modelBuilder);

        #endregion

        #region CharacterState Mapping

        private void CharacterStateMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterState>().ToTable(@"character_state", @"game");
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.CharacterId).HasColumnName(@"character_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.Hp).HasColumnName(@"hp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.Mp).HasColumnName(@"mp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.Stamina).HasColumnName(@"stamina").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.MaxHp).HasColumnName(@"max_hp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.MaxMp).HasColumnName(@"max_mp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.MaxStamina).HasColumnName(@"max_stamina").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int?>(x => x.EquipedWeaponId).HasColumnName(@"equiped_weapon_id").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int?>(x => x.EquipedArmorId).HasColumnName(@"equiped_armor_id").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int?>(x => x.EquipedAmuletId).HasColumnName(@"equiped_amulet_id").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.XPos).HasColumnName(@"x_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int>(x => x.YPos).HasColumnName(@"y_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().Property<int?>(x => x.EquipedOffhandId).HasColumnName(@"equiped_offhand_id").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<CharacterState>().HasKey(@"CharacterId");
        }

        partial void CustomizeCharacterStateMapping(ModelBuilder modelBuilder);

        #endregion

        #region ExistingItem Mapping

        private void ExistingItemMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExistingItem>().ToTable(@"existing_item", @"game");
            modelBuilder.Entity<ExistingItem>().Property<int>(x => x.ExistingItemId).HasColumnName(@"existing_item_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingItem>().Property<int>(x => x.ItemId).HasColumnName(@"item_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingItem>().Property<int>(x => x.ItemTypeId).HasColumnName(@"item_type_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingItem>().Property<int?>(x => x.CharacterId).HasColumnName(@"character_id").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ExistingItem>().HasKey(@"ExistingItemId");
        }

        partial void CustomizeExistingItemMapping(ModelBuilder modelBuilder);

        #endregion

        #region ExistingMonster Mapping

        private void ExistingMonsterMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExistingMonster>().ToTable(@"existing_monster", @"game");
            modelBuilder.Entity<ExistingMonster>().Property<int>(x => x.ExistingMonsterId).HasColumnName(@"existing_monster_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingMonster>().Property<int>(x => x.MonsterId).HasColumnName(@"monster_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingMonster>().Property<int>(x => x.XPos).HasColumnName(@"x_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingMonster>().Property<int>(x => x.YPos).HasColumnName(@"y_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingMonster>().Property<int>(x => x.Damaged).HasColumnName(@"damaged").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ExistingMonster>().HasKey(@"ExistingMonsterId");
        }

        partial void CustomizeExistingMonsterMapping(ModelBuilder modelBuilder);

        #endregion

        #region Item Mapping

        private void ItemMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable(@"item", @"game");
            modelBuilder.Entity<Item>().Property<int>(x => x.ItemId).HasColumnName(@"item_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.Damage).HasColumnName(@"damage").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.Armor).HasColumnName(@"armor").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.StrBonus).HasColumnName(@"str_bonus").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.DexBonus).HasColumnName(@"dex_bonus").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.IntBonus).HasColumnName(@"int_bonus").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.ConBonus).HasColumnName(@"con_bonus").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int?>(x => x.ChaBonus).HasColumnName(@"cha_bonus").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<string>(x => x.Name).HasColumnName(@"name").HasColumnType(@"varchar").ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<Item>().Property<int>(x => x.Weight).HasColumnName(@"weight").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int>(x => x.Worth).HasColumnName(@"worth").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<string>(x => x.Type).HasColumnName(@"type").HasColumnType(@"enum").ValueGeneratedNever();
            modelBuilder.Entity<Item>().Property<int>(x => x.RarityLevel).HasColumnName(@"rarity_level").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Item>().HasKey(@"ItemId");
        }

        partial void CustomizeItemMapping(ModelBuilder modelBuilder);

        #endregion

        #region ItemType Mapping

        private void ItemTypeMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemType>().ToTable(@"item_type", @"game");
            modelBuilder.Entity<ItemType>().Property<int>(x => x.ItemTypeId).HasColumnName(@"item_type_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<string>(x => x.ItemTypeName).HasColumnName(@"item_type_name").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<ItemType>().Property<double>(x => x.DamageModifier).HasColumnName(@"damage_modifier").HasColumnType(@"double").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<double>(x => x.ArmorModifier).HasColumnName(@"armor_modifier").HasColumnType(@"double").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int?>(x => x.StrModifier).HasColumnName(@"str_modifier").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int?>(x => x.DexModifier).HasColumnName(@"dex_modifier").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int?>(x => x.IntModifier).HasColumnName(@"int_modifier").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int?>(x => x.ConModifier).HasColumnName(@"con_modifier").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int?>(x => x.ChaModifier).HasColumnName(@"cha_modifier").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<double>(x => x.WorthModifier).HasColumnName(@"worth_modifier").HasColumnType(@"double").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<double>(x => x.WeightModifier).HasColumnName(@"weight_modifier").HasColumnType(@"double").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int>(x => x.TypeRarityLevel).HasColumnName(@"type_rarity_level").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().Property<int>(x => x.AdditionalWorth).HasColumnName(@"additional_worth").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ItemType>().HasKey(@"ItemTypeId");
        }

        partial void CustomizeItemTypeMapping(ModelBuilder modelBuilder);

        #endregion

        #region Monster Mapping

        private void MonsterMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Monster>().ToTable(@"monster", @"game");
            modelBuilder.Entity<Monster>().Property<int>(x => x.MonsterId).HasColumnName(@"monster_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<string>(x => x.MonsterName).HasColumnName(@"monster_name").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<Monster>().Property<int>(x => x.Str).HasColumnName(@"str").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Dex).HasColumnName(@"dex").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Int).HasColumnName(@"int").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Con).HasColumnName(@"con").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Cha).HasColumnName(@"cha").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Hp).HasColumnName(@"hp").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Damage).HasColumnName(@"damage").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.Armor).HasColumnName(@"armor").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<bool>(x => x.InMeadow).HasColumnName(@"inMeadow").HasColumnType(@"tinyint").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<bool>(x => x.InForest).HasColumnName(@"inForest").HasColumnType(@"tinyint").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<bool>(x => x.InDesert).HasColumnName(@"inDesert").HasColumnType(@"tinyint").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<bool>(x => x.InTown).HasColumnName(@"inTown").HasColumnType(@"tinyint").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().Property<int>(x => x.RarityLevel).HasColumnName(@"rarityLevel").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Monster>().HasKey(@"MonsterId");
        }

        partial void CustomizeMonsterMapping(ModelBuilder modelBuilder);

        #endregion

        #region Merchant Mapping

        private void MerchantMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Merchant>().ToTable(@"merchant", @"game");
            modelBuilder.Entity<Merchant>().Property<int>(x => x.Gold).HasColumnName(@"gold").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<int?>(x => x.MealWorth).HasColumnName(@"meal_worth").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<int>(x => x.MerchantId).HasColumnName(@"merchant_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<string>(x => x.Name).HasColumnName(@"name").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<Merchant>().Property<int?>(x => x.RoomRentWorth).HasColumnName(@"room_rent_worth").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<string>(x => x.ShopName).HasColumnName(@"shop_name").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<Merchant>().Property<string>(x => x.Type).HasColumnName(@"type").HasColumnType(@"enum").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<int>(x => x.XPos).HasColumnName(@"XPos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().Property<int>(x => x.YPos).HasColumnName(@"YPos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Merchant>().HasKey(@"MerchantId");
        }

        partial void CustomizeMerchantMapping(ModelBuilder modelBuilder);

        #endregion

        #region User Mapping

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(@"user", @"game");
            modelBuilder.Entity<User>().Property<int>(x => x.UserId).HasColumnName(@"user_id").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<User>().Property<string>(x => x.Email).HasColumnName(@"email").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<User>().Property<string>(x => x.Login).HasColumnName(@"login").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(45);
            modelBuilder.Entity<User>().Property<string>(x => x.PasswordHash).HasColumnName(@"password_hash").HasColumnType(@"varchar").IsRequired().ValueGeneratedNever().HasMaxLength(100);
            modelBuilder.Entity<User>().HasKey(@"UserId");
        }

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        #endregion

        #region Zone Mapping

        private void ZoneMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Zone>().ToTable(@"zone", @"game");
            modelBuilder.Entity<Zone>().Property<int>(x => x.XPos).HasColumnName(@"x_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Zone>().Property<int>(x => x.YPos).HasColumnName(@"y_pos").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Zone>().Property<int>(x => x.Type).HasColumnName(@"type").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Zone>().HasKey(@"XPos", @"YPos");
        }

        partial void CustomizeZoneMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {

        #region Character Navigation properties

            modelBuilder.Entity<Character>().HasOne(x => x.User).WithMany(op => op.Characters).IsRequired(true).HasForeignKey(@"UserId");
            modelBuilder.Entity<Character>().HasOne(x => x.CharacterInfo).WithOne(op => op.Character).IsRequired(true).HasForeignKey(typeof(CharacterInfo), @"CharacterId");
            modelBuilder.Entity<Character>().HasOne(x => x.CharacterState).WithOne(op => op.Character).IsRequired(true).HasForeignKey(typeof(CharacterState), @"CharacterId");
            modelBuilder.Entity<Character>().HasMany(x => x.ExistingItems).WithOne(op => op.Character).IsRequired(false).HasForeignKey(@"CharacterId");

            #endregion

        #region CharacterInfo Navigation properties

            modelBuilder.Entity<CharacterInfo>().HasOne(x => x.Character).WithOne(op => op.CharacterInfo).IsRequired(true).HasForeignKey(typeof(CharacterInfo), @"CharacterId");

        #endregion

        #region CharacterState Navigation properties

            modelBuilder.Entity<CharacterState>().HasOne(x => x.Character).WithOne(op => op.CharacterState).IsRequired(true).HasForeignKey(typeof(CharacterState), @"CharacterId");
            modelBuilder.Entity<CharacterState>().HasOne(x => x.ExistingItem_EquipedAmuletId).WithMany(op => op.CharacterStates_EquipedAmuletId).IsRequired(false).HasForeignKey(@"EquipedAmuletId");
            modelBuilder.Entity<CharacterState>().HasOne(x => x.ExistingItem_EquipedArmorId).WithMany(op => op.CharacterStates_EquipedArmorId).IsRequired(false).HasForeignKey(@"EquipedArmorId");
            modelBuilder.Entity<CharacterState>().HasOne(x => x.ExistingItem_EquipedOffhandId).WithMany(op => op.CharacterStates_EquipedOffhandId).IsRequired(false).HasForeignKey(@"EquipedOffhandId");
            modelBuilder.Entity<CharacterState>().HasOne(x => x.ExistingItem_EquipedWeaponId).WithMany(op => op.CharacterStates_EquipedWeaponId).IsRequired(false).HasForeignKey(@"EquipedWeaponId");

        #endregion

        #region ExistingItem Navigation properties

            modelBuilder.Entity<ExistingItem>().HasMany(x => x.CharacterStates_EquipedAmuletId).WithOne(op => op.ExistingItem_EquipedAmuletId).IsRequired(false).HasForeignKey(@"EquipedAmuletId");
            modelBuilder.Entity<ExistingItem>().HasMany(x => x.CharacterStates_EquipedArmorId).WithOne(op => op.ExistingItem_EquipedArmorId).IsRequired(false).HasForeignKey(@"EquipedArmorId");
            modelBuilder.Entity<ExistingItem>().HasMany(x => x.CharacterStates_EquipedOffhandId).WithOne(op => op.ExistingItem_EquipedOffhandId).IsRequired(false).HasForeignKey(@"EquipedOffhandId");
            modelBuilder.Entity<ExistingItem>().HasMany(x => x.CharacterStates_EquipedWeaponId).WithOne(op => op.ExistingItem_EquipedWeaponId).IsRequired(false).HasForeignKey(@"EquipedWeaponId");
            modelBuilder.Entity<ExistingItem>().HasOne(x => x.Item).WithMany(op => op.ExistingItems).IsRequired(true).HasForeignKey(@"ItemId");
            modelBuilder.Entity<ExistingItem>().HasOne(x => x.Character).WithMany(op => op.ExistingItems).IsRequired(false).HasForeignKey(@"CharacterId");
            modelBuilder.Entity<ExistingItem>().HasOne(x => x.ItemType).WithMany(op => op.ExistingItems).IsRequired(true).HasForeignKey(@"ItemTypeId");

        #endregion

        #region ExistingMonster Navigation properties

            modelBuilder.Entity<ExistingMonster>().HasOne(x => x.Monster).WithMany(op => op.ExistingMonsters).IsRequired(true).HasForeignKey(@"MonsterId");

        #endregion

        #region Item Navigation properties

            modelBuilder.Entity<Item>().HasMany(x => x.ExistingItems).WithOne(op => op.Item).IsRequired(true).HasForeignKey(@"ItemId");

        #endregion

        #region ItemType Navigation properties

            modelBuilder.Entity<ItemType>().HasMany(x => x.ExistingItems).WithOne(op => op.ItemType).IsRequired(true).HasForeignKey(@"ItemTypeId");

        #endregion

        #region Monster Navigation properties

            modelBuilder.Entity<Monster>().HasMany(x => x.ExistingMonsters).WithOne(op => op.Monster).IsRequired(true).HasForeignKey(@"MonsterId");

            #endregion

        #region Merchant Navigation properties


            #endregion

        #region User Navigation properties

            modelBuilder.Entity<User>().HasMany(x => x.Characters).WithOne(op => op.User).IsRequired(true).HasForeignKey(@"UserId");

        #endregion
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
