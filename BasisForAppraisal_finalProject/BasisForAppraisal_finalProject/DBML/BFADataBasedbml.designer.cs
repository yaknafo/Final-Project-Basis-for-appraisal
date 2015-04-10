﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BasisForAppraisal_finalProject.DBML
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="BasisForAppraisalDB")]
	public partial class BFADataBasedbmlDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InserttblForm(tblForm instance);
    partial void UpdatetblForm(tblForm instance);
    partial void DeletetblForm(tblForm instance);
    partial void Inserttbl_IntentionalQuestion(tbl_IntentionalQuestion instance);
    partial void Updatetbl_IntentionalQuestion(tbl_IntentionalQuestion instance);
    partial void Deletetbl_IntentionalQuestion(tbl_IntentionalQuestion instance);
    partial void Inserttbl_IntentionalAnswer(tbl_IntentionalAnswer instance);
    partial void Updatetbl_IntentionalAnswer(tbl_IntentionalAnswer instance);
    partial void Deletetbl_IntentionalAnswer(tbl_IntentionalAnswer instance);
    partial void Inserttbl_Company(tbl_Company instance);
    partial void Updatetbl_Company(tbl_Company instance);
    partial void Deletetbl_Company(tbl_Company instance);
    partial void Inserttbl_Employee(tbl_Employee instance);
    partial void Updatetbl_Employee(tbl_Employee instance);
    partial void Deletetbl_Employee(tbl_Employee instance);
    #endregion
		
		public BFADataBasedbmlDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["BasisForAppraisalDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public BFADataBasedbmlDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BFADataBasedbmlDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BFADataBasedbmlDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BFADataBasedbmlDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<tblForm> tblForms
		{
			get
			{
				return this.GetTable<tblForm>();
			}
		}
		
		public System.Data.Linq.Table<tbl_IntentionalQuestion> tbl_IntentionalQuestions
		{
			get
			{
				return this.GetTable<tbl_IntentionalQuestion>();
			}
		}
		
		public System.Data.Linq.Table<tbl_IntentionalAnswer> tbl_IntentionalAnswers
		{
			get
			{
				return this.GetTable<tbl_IntentionalAnswer>();
			}
		}
		
		public System.Data.Linq.Table<tbl_Company> tbl_Companies
		{
			get
			{
				return this.GetTable<tbl_Company>();
			}
		}
		
		public System.Data.Linq.Table<tbl_Employee> tbl_Employees
		{
			get
			{
				return this.GetTable<tbl_Employee>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tblForm")]
	public partial class tblForm : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FormId;
		
		private string _FormName;
		
		private EntitySet<tbl_IntentionalQuestion> _tbl_IntentionalQuestions;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnformIdChanging(int value);
    partial void OnformIdChanged();
    partial void OnFormNameChanging(string value);
    partial void OnFormNameChanged();
    #endregion
		
		public tblForm()
		{
			this._tbl_IntentionalQuestions = new EntitySet<tbl_IntentionalQuestion>(new Action<tbl_IntentionalQuestion>(this.attach_tbl_IntentionalQuestions), new Action<tbl_IntentionalQuestion>(this.detach_tbl_IntentionalQuestions));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="formId", Storage="_FormId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int formId
		{
			get
			{
				return this._FormId;
			}
			set
			{
				if ((this._FormId != value))
				{
					this.OnformIdChanging(value);
					this.SendPropertyChanging();
					this._FormId = value;
					this.SendPropertyChanged("formId");
					this.OnformIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormName", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string FormName
		{
			get
			{
				return this._FormName;
			}
			set
			{
				if ((this._FormName != value))
				{
					this.OnFormNameChanging(value);
					this.SendPropertyChanging();
					this._FormName = value;
					this.SendPropertyChanged("FormName");
					this.OnFormNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tblForm_tbl_IntentionalQuestion", Storage="_tbl_IntentionalQuestions", ThisKey="formId", OtherKey="FormId")]
		public EntitySet<tbl_IntentionalQuestion> tbl_IntentionalQuestions
		{
			get
			{
				return this._tbl_IntentionalQuestions;
			}
			set
			{
				this._tbl_IntentionalQuestions.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_tbl_IntentionalQuestions(tbl_IntentionalQuestion entity)
		{
			this.SendPropertyChanging();
			entity.tblForm = this;
		}
		
		private void detach_tbl_IntentionalQuestions(tbl_IntentionalQuestion entity)
		{
			this.SendPropertyChanging();
			entity.tblForm = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbl_IntentionalQuestion")]
	public partial class tbl_IntentionalQuestion : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FormId;
		
		private int _QuestionId;
		
		private string _Title;
		
		private EntitySet<tbl_IntentionalAnswer> _tbl_IntentionalAnswers;
		
		private EntityRef<tblForm> _tblForm;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFormIdChanging(int value);
    partial void OnFormIdChanged();
    partial void OnQuestionIdChanging(int value);
    partial void OnQuestionIdChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    #endregion
		
		public tbl_IntentionalQuestion()
		{
			this._tbl_IntentionalAnswers = new EntitySet<tbl_IntentionalAnswer>(new Action<tbl_IntentionalAnswer>(this.attach_tbl_IntentionalAnswers), new Action<tbl_IntentionalAnswer>(this.detach_tbl_IntentionalAnswers));
			this._tblForm = default(EntityRef<tblForm>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int FormId
		{
			get
			{
				return this._FormId;
			}
			set
			{
				if ((this._FormId != value))
				{
					if (this._tblForm.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnFormIdChanging(value);
					this.SendPropertyChanging();
					this._FormId = value;
					this.SendPropertyChanged("FormId");
					this.OnFormIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestionId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int QuestionId
		{
			get
			{
				return this._QuestionId;
			}
			set
			{
				if ((this._QuestionId != value))
				{
					this.OnQuestionIdChanging(value);
					this.SendPropertyChanging();
					this._QuestionId = value;
					this.SendPropertyChanged("QuestionId");
					this.OnQuestionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="VarChar(300)")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tbl_IntentionalQuestion_tbl_IntentionalAnswer", Storage="_tbl_IntentionalAnswers", ThisKey="FormId,QuestionId", OtherKey="FormId,QuestionId")]
		public EntitySet<tbl_IntentionalAnswer> tbl_IntentionalAnswers
		{
			get
			{
				return this._tbl_IntentionalAnswers;
			}
			set
			{
				this._tbl_IntentionalAnswers.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tblForm_tbl_IntentionalQuestion", Storage="_tblForm", ThisKey="FormId", OtherKey="formId", IsForeignKey=true)]
		public tblForm tblForm
		{
			get
			{
				return this._tblForm.Entity;
			}
			set
			{
				tblForm previousValue = this._tblForm.Entity;
				if (((previousValue != value) 
							|| (this._tblForm.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._tblForm.Entity = null;
						previousValue.tbl_IntentionalQuestions.Remove(this);
					}
					this._tblForm.Entity = value;
					if ((value != null))
					{
						value.tbl_IntentionalQuestions.Add(this);
						this._FormId = value.formId;
					}
					else
					{
						this._FormId = default(int);
					}
					this.SendPropertyChanged("tblForm");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_tbl_IntentionalAnswers(tbl_IntentionalAnswer entity)
		{
			this.SendPropertyChanging();
			entity.tbl_IntentionalQuestion = this;
		}
		
		private void detach_tbl_IntentionalAnswers(tbl_IntentionalAnswer entity)
		{
			this.SendPropertyChanging();
			entity.tbl_IntentionalQuestion = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbl_IntentionalAnswer")]
	public partial class tbl_IntentionalAnswer : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FormId;
		
		private int _QuestionId;
		
		private int _AnswerId;
		
		private string _Text;
		
		private System.Nullable<bool> _AnswerOption;
		
		private System.Nullable<int> _Score;
		
		private EntityRef<tbl_IntentionalQuestion> _tbl_IntentionalQuestion;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFormIdChanging(int value);
    partial void OnFormIdChanged();
    partial void OnQuestionIdChanging(int value);
    partial void OnQuestionIdChanged();
    partial void OnAnswerIdChanging(int value);
    partial void OnAnswerIdChanged();
    partial void OnTextChanging(string value);
    partial void OnTextChanged();
    partial void OnAnswerOptionChanging(System.Nullable<bool> value);
    partial void OnAnswerOptionChanged();
    partial void OnScoreChanging(System.Nullable<int> value);
    partial void OnScoreChanged();
    #endregion
		
		public tbl_IntentionalAnswer()
		{
			this._tbl_IntentionalQuestion = default(EntityRef<tbl_IntentionalQuestion>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int FormId
		{
			get
			{
				return this._FormId;
			}
			set
			{
				if ((this._FormId != value))
				{
					if (this._tbl_IntentionalQuestion.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnFormIdChanging(value);
					this.SendPropertyChanging();
					this._FormId = value;
					this.SendPropertyChanged("FormId");
					this.OnFormIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestionId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int QuestionId
		{
			get
			{
				return this._QuestionId;
			}
			set
			{
				if ((this._QuestionId != value))
				{
					if (this._tbl_IntentionalQuestion.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnQuestionIdChanging(value);
					this.SendPropertyChanging();
					this._QuestionId = value;
					this.SendPropertyChanged("QuestionId");
					this.OnQuestionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnswerId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int AnswerId
		{
			get
			{
				return this._AnswerId;
			}
			set
			{
				if ((this._AnswerId != value))
				{
					this.OnAnswerIdChanging(value);
					this.SendPropertyChanging();
					this._AnswerId = value;
					this.SendPropertyChanged("AnswerId");
					this.OnAnswerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Text", DbType="VarChar(500)")]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if ((this._Text != value))
				{
					this.OnTextChanging(value);
					this.SendPropertyChanging();
					this._Text = value;
					this.SendPropertyChanged("Text");
					this.OnTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnswerOption", DbType="Bit")]
		public System.Nullable<bool> AnswerOption
		{
			get
			{
				return this._AnswerOption;
			}
			set
			{
				if ((this._AnswerOption != value))
				{
					this.OnAnswerOptionChanging(value);
					this.SendPropertyChanging();
					this._AnswerOption = value;
					this.SendPropertyChanged("AnswerOption");
					this.OnAnswerOptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Score", DbType="Int")]
		public System.Nullable<int> Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				if ((this._Score != value))
				{
					this.OnScoreChanging(value);
					this.SendPropertyChanging();
					this._Score = value;
					this.SendPropertyChanged("Score");
					this.OnScoreChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tbl_IntentionalQuestion_tbl_IntentionalAnswer", Storage="_tbl_IntentionalQuestion", ThisKey="FormId,QuestionId", OtherKey="FormId,QuestionId", IsForeignKey=true)]
		public tbl_IntentionalQuestion tbl_IntentionalQuestion
		{
			get
			{
				return this._tbl_IntentionalQuestion.Entity;
			}
			set
			{
				tbl_IntentionalQuestion previousValue = this._tbl_IntentionalQuestion.Entity;
				if (((previousValue != value) 
							|| (this._tbl_IntentionalQuestion.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._tbl_IntentionalQuestion.Entity = null;
						previousValue.tbl_IntentionalAnswers.Remove(this);
					}
					this._tbl_IntentionalQuestion.Entity = value;
					if ((value != null))
					{
						value.tbl_IntentionalAnswers.Add(this);
						this._FormId = value.FormId;
						this._QuestionId = value.QuestionId;
					}
					else
					{
						this._FormId = default(int);
						this._QuestionId = default(int);
					}
					this.SendPropertyChanged("tbl_IntentionalQuestion");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbl_Company")]
	public partial class tbl_Company : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _companyId;
		
		private string _comapnyName;
		
		private string _comapnyAddress;
		
		private string _comapnyPhone;
		
		private EntitySet<tbl_Employee> _tbl_Employees;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OncompanyIdChanging(int value);
    partial void OncompanyIdChanged();
    partial void OncomapnyNameChanging(string value);
    partial void OncomapnyNameChanged();
    partial void OncomapnyAddressChanging(string value);
    partial void OncomapnyAddressChanged();
    partial void OncomapnyPhoneChanging(string value);
    partial void OncomapnyPhoneChanged();
    #endregion
		
		public tbl_Company()
		{
			this._tbl_Employees = new EntitySet<tbl_Employee>(new Action<tbl_Employee>(this.attach_tbl_Employees), new Action<tbl_Employee>(this.detach_tbl_Employees));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_companyId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int companyId
		{
			get
			{
				return this._companyId;
			}
			set
			{
				if ((this._companyId != value))
				{
					this.OncompanyIdChanging(value);
					this.SendPropertyChanging();
					this._companyId = value;
					this.SendPropertyChanged("companyId");
					this.OncompanyIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comapnyName", DbType="VarChar(100)")]
		public string comapnyName
		{
			get
			{
				return this._comapnyName;
			}
			set
			{
				if ((this._comapnyName != value))
				{
					this.OncomapnyNameChanging(value);
					this.SendPropertyChanging();
					this._comapnyName = value;
					this.SendPropertyChanged("comapnyName");
					this.OncomapnyNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comapnyAddress", DbType="VarChar(300)")]
		public string comapnyAddress
		{
			get
			{
				return this._comapnyAddress;
			}
			set
			{
				if ((this._comapnyAddress != value))
				{
					this.OncomapnyAddressChanging(value);
					this.SendPropertyChanging();
					this._comapnyAddress = value;
					this.SendPropertyChanged("comapnyAddress");
					this.OncomapnyAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comapnyPhone", DbType="VarChar(50)")]
		public string comapnyPhone
		{
			get
			{
				return this._comapnyPhone;
			}
			set
			{
				if ((this._comapnyPhone != value))
				{
					this.OncomapnyPhoneChanging(value);
					this.SendPropertyChanging();
					this._comapnyPhone = value;
					this.SendPropertyChanged("comapnyPhone");
					this.OncomapnyPhoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tbl_Company_tbl_Employee", Storage="_tbl_Employees", ThisKey="companyId", OtherKey="companyId")]
		public EntitySet<tbl_Employee> tbl_Employees
		{
			get
			{
				return this._tbl_Employees;
			}
			set
			{
				this._tbl_Employees.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_tbl_Employees(tbl_Employee entity)
		{
			this.SendPropertyChanging();
			entity.tbl_Company = this;
		}
		
		private void detach_tbl_Employees(tbl_Employee entity)
		{
			this.SendPropertyChanging();
			entity.tbl_Company = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbl_Employee")]
	public partial class tbl_Employee : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _companyId;
		
		private string _employeeId;
		
		private string _firstName;
		
		private string _lastName;
		
		private EntityRef<tbl_Company> _tbl_Company;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OncompanyIdChanging(int value);
    partial void OncompanyIdChanged();
    partial void OnemployeeIdChanging(string value);
    partial void OnemployeeIdChanged();
    partial void OnfirstNameChanging(string value);
    partial void OnfirstNameChanged();
    partial void OnlastNameChanging(string value);
    partial void OnlastNameChanged();
    #endregion
		
		public tbl_Employee()
		{
			this._tbl_Company = default(EntityRef<tbl_Company>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_companyId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int companyId
		{
			get
			{
				return this._companyId;
			}
			set
			{
				if ((this._companyId != value))
				{
					if (this._tbl_Company.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OncompanyIdChanging(value);
					this.SendPropertyChanging();
					this._companyId = value;
					this.SendPropertyChanged("companyId");
					this.OncompanyIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_employeeId", DbType="VarChar(9) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string employeeId
		{
			get
			{
				return this._employeeId;
			}
			set
			{
				if ((this._employeeId != value))
				{
					this.OnemployeeIdChanging(value);
					this.SendPropertyChanging();
					this._employeeId = value;
					this.SendPropertyChanged("employeeId");
					this.OnemployeeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_firstName", DbType="VarChar(30)")]
		public string firstName
		{
			get
			{
				return this._firstName;
			}
			set
			{
				if ((this._firstName != value))
				{
					this.OnfirstNameChanging(value);
					this.SendPropertyChanging();
					this._firstName = value;
					this.SendPropertyChanged("firstName");
					this.OnfirstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_lastName", DbType="VarChar(30)")]
		public string lastName
		{
			get
			{
				return this._lastName;
			}
			set
			{
				if ((this._lastName != value))
				{
					this.OnlastNameChanging(value);
					this.SendPropertyChanging();
					this._lastName = value;
					this.SendPropertyChanged("lastName");
					this.OnlastNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="tbl_Company_tbl_Employee", Storage="_tbl_Company", ThisKey="companyId", OtherKey="companyId", IsForeignKey=true)]
		public tbl_Company tbl_Company
		{
			get
			{
				return this._tbl_Company.Entity;
			}
			set
			{
				tbl_Company previousValue = this._tbl_Company.Entity;
				if (((previousValue != value) 
							|| (this._tbl_Company.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._tbl_Company.Entity = null;
						previousValue.tbl_Employees.Remove(this);
					}
					this._tbl_Company.Entity = value;
					if ((value != null))
					{
						value.tbl_Employees.Add(this);
						this._companyId = value.companyId;
					}
					else
					{
						this._companyId = default(int);
					}
					this.SendPropertyChanged("tbl_Company");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
