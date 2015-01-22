I. Index
----------------------------------------------

I. Index
II. Using the Scrambler
	a. MVC Projects
	b. Web API
III. Attributes
	a. Adding Attributes
	b. Creating new Attributes
IV. Attribute Registration 


II. Using the Scrambler
----------------------------------------------
	The Scrambler is intended to be easy to implement. As such, simply including the library
	and setting it to be on is enough for it to automatically obfuscate all data being passed.
	The ObfuscationSettings.IsActiveDefault flag can be used to make it on at a global level; 
	just set it to true somewhere in the application and everything after that until it is set 
	to false will be scrambled. 

	a. MVC Projects

		In order to get the Scrambler to work for MVC pages, the ScramblerMvcAttribute needs
		to decorate any views or controllers which should be obfuscated. Alternately, the 
		attribute can be applied to all controllers automatically with the following like in 
		the Global.asax.

			GlobalFilters.Filters.Add(new ScramblerMvcAttribute());

		Using the IsActiveDefault flag can be undesirable in server-side applications, as it 
		will control all applications which are active, meaning it can change other user 
		sessions. Instead, it is better to use the ScrambleActiveCookie, which will store 
		the information per-session. This can be set directly in the code somewhere or by 
		using the ScramblerMvcAttribute, which allows the cookie to be set with a query 
		specifying "true" or "false" for the attribute, with the key for the query set in 
		the argument. Additionally, it can be registered like the ScramblerMvcAttribute above
		to the Global.asax:

			GlobalFilters.Filters.Add(new ScramblerMvcSessionSwitchAttribute(ArgumentName));

	b. Web API

		Like the MVC projects above, the ScramblerWebApiAttribute needs to decorate the Web API
		functions or controllers which are to be obfuscated, which can again be done in the 
		Global.asax:

			GlobalConfiguration.Configuration.Filters.Add(new ScramblerWebApiAttribute())

		Likewise, the ScrambleActiveCookie is preferred as the setting to use and can be set using 
		the same methods as in MVC projects, with the ScramblerWebApiSessionSwitchAttribute being 
		used to register compatible pages instead, or the following in the Global.asax:

			GlobalFilters.Filters.Add(new ScramblerWebApiSessionSwitchAttribute(ArgumentName));


III. ScrambleAttributes
----------------------------------------------
	ScrambleAttributes are used to specify how a field or property should be obfuscated. 
	In general, they are used when the default scrambler is not able to properly detect
	what the value should be, or strict detection is not desired for a field or property.

	a. Adding Attributes

		Adding a ScrambleAttribute to a class is the same as adding any attribute to a
		class; just add it before the field or property (or, if all elements in a class
		should be obfuscated the same way, to the class itself) and pass in any relevant 
		arguments as necessary. Normally, the only times an entire class would share a
		ScrambleAttribute is if the class contains a number of elements all of the same 
		type or if the entire class should not be scrambled.

		If you want an element not to be scrambled, you can choose either to use the 
		ScrambleIgnoreAttribute or set the Ignore flag in a Scramble attribute to true.
		This will allow the field, property, or class to be passed as-is from the 
		application.

		You can also set the IsStrict flag directly as you can the Ignore; however, if 
		a ScrambleAttribute has Strict settings that is uses, it will usually have a 
		constructor which will use the IsStrict flag and will otherwise ignore it.

	b. Creating new Attributes

		Creating custom attributes may be necessary when something which must maintain a 
		certain pattern while being obfuscated; in these cases, it may be necessary to 
		create a custom ScrambleAttribute.

		In order to create a new ScrambleAttribute, your new Attribute should extend 
		CCHMC.Core.Web.Scrambler.Attributes.ScrambleAttribute and override the Obfuscate
		function and/or set the _obfuscate object elsewhere, such as in the constructor(s).

		When using the IsStrict setting, you will usually want to check it in the Obfuscate
		function, where the value which is being obfuscated is stored in the "obj" argument
		and can be checked to detect for anything which should be retained when the strict
		setting is on. 

		When you generate the value, either in the constructor or in the IsStrict setting, 
		it should normally be stored in the _obfuscate property, and the Obfuscate function
		should check to make sure it is null before generating a new value. By only changing
		the return value when it hasn't been initialized, the value will remain constant on 
		each page whenever it is called for a given object. Regenerating the value will 
		cause the value to be inconsistent when loaded multiple times, so only do so if that 
		is the desired behaviour.


IV. Attribute Registration 
----------------------------------------------
	In some cases, attributes cannot be set directly to a class. This typically occurs when 
	the class is either part of an external library, so the source is inaccessible, or when
	the class is automatically generated and any changes may be lost. In these cases, use 
	the AttributeRegister helper to add Attributes to the class and/or its properties.

	All registration for types and members should be done at the beginning of the application,
	and only done once. Multiple registrations for a type or member will just overwrite it, 
	leaving the last-registered Attribute applied.

	Attributes can be registered to a class by calling AttributeRegister.RegisterType (or with
	AttributeRegister.IgnoreType if it should be ignored):

		//Add the ExampleScrambleAttribute to the type.
		AttributeRegister.RegisterType(Type, ExampleScrambleAttribute);
		//Ignore the Type when scrambling.
		AttributeRegister.IgnoreType(Type);

	Similarly, members of a class can be registered with AttributeRegister.RegisterMember (or
	AttributeRegister.IgnoreMember):

		//Add the ExampleScrambleAttribute to the type.
		AttributeRegister.RegisterMember(Type, MemberName, ExampleScrambleAttribute);
		//Ignore the member when scrambling.
		AttributeRegister.IgnoreMember(Type, MemberName);

	All arguments for the ScrambleAttribute should be passed in when it is initialized before 
	being passed in to the Register.