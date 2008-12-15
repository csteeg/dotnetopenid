﻿//-----------------------------------------------------------------------
// <copyright file="NegativeAssertionResponseTests.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.Test.OpenId.Messages {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using DotNetOpenAuth.Messaging;
	using DotNetOpenAuth.OpenId;
	using DotNetOpenAuth.OpenId.Messages;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class NegativeAssertionResponseTests : OpenIdTestBase {
		[TestInitialize]
		public override void SetUp() {
			base.SetUp();
		}

		[TestMethod]
		public void Mode() {
			var setupRequestV1 = new CheckIdRequest(Protocol.V10.Version, ProviderUri, false);
			setupRequestV1.ReturnTo = RPUri;
			var immediateRequestV1 = new CheckIdRequest(Protocol.V10.Version, ProviderUri, true);
			immediateRequestV1.ReturnTo = RPUri;

			var setupRequestV2 = new CheckIdRequest(Protocol.V20.Version, ProviderUri, false);
			setupRequestV2.ReturnTo = RPUri;
			var immediateRequestV2 = new CheckIdRequest(Protocol.V20.Version, ProviderUri, true);
			immediateRequestV2.ReturnTo = RPUri;

			Assert.AreEqual("id_res", new NegativeAssertionResponse(immediateRequestV1).Mode);
			Assert.AreEqual("cancel", new NegativeAssertionResponse(setupRequestV1).Mode);
			Assert.AreEqual("setup_needed", new NegativeAssertionResponse(immediateRequestV2).Mode);
			Assert.AreEqual("cancel", new NegativeAssertionResponse(setupRequestV2).Mode);
		}

		[TestMethod, ExpectedException(typeof(ProtocolException))]
		public void UserSetupUrlRequiredInV1Immediate() {
			var immediateRequestV1 = new CheckIdRequest(Protocol.V10.Version, ProviderUri, true);
			immediateRequestV1.ReturnTo = RPUri;
			new NegativeAssertionResponse(immediateRequestV1).EnsureValidMessage();
		}

		[TestMethod]
		public void UserSetupUrlSetForV1Immediate() {
			var immediateRequestV1 = new CheckIdRequest(Protocol.V10.Version, ProviderUri, true);
			immediateRequestV1.ReturnTo = RPUri;
			var response = new NegativeAssertionResponse(immediateRequestV1);
			response.UserSetupUrl = new Uri("http://usersetup");
			response.EnsureValidMessage();
		}

		[TestMethod]
		public void UserSetupUrlNotRequiredInV1SetupOrV2() {
			var setupRequestV1 = new CheckIdRequest(Protocol.V10.Version, ProviderUri, false);
			setupRequestV1.ReturnTo = RPUri;
			new NegativeAssertionResponse(setupRequestV1).EnsureValidMessage();

			var setupRequestV2 = new CheckIdRequest(Protocol.V20.Version, ProviderUri, false);
			setupRequestV2.ReturnTo = RPUri;
			new NegativeAssertionResponse(setupRequestV2).EnsureValidMessage();

			var immediateRequestV2 = new CheckIdRequest(Protocol.V20.Version, ProviderUri, true);
			immediateRequestV2.ReturnTo = RPUri;
			new NegativeAssertionResponse(immediateRequestV2).EnsureValidMessage();
		}
	}
}