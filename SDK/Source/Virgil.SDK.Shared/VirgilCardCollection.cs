#region Copyright (C) Virgil Security Inc.
// Copyright (C) 2015-2016 Virgil Security Inc.
// 
// Lead Maintainer: Virgil Security Inc. <support@virgilsecurity.com>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions 
// are met:
// 
//   (1) Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
//   
//   (2) Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in
//   the documentation and/or other materials provided with the
//   distribution.
//   
//   (3) Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived 
//   from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ''AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Virgil.SDK
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class VirgilCardCollection : IReadOnlyCollection<VirgilCard> //  IEnumerable<VirgilCard>
	{
		private readonly VirgilApiContext context;
		private readonly IList<VirgilCard> cards;

		/// <summary>
		/// Initializes a new instance of the <see cref="VirgilCardCollection"/> class.
		/// </summary>
		public VirgilCardCollection(VirgilApiContext context, IList<VirgilCard> cards)
		{
			this.context = context;
			this.cards = cards;
		}

		public IEnumerator<VirgilCard> GetEnumerator()
		{
			return this.cards.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.cards.GetEnumerator();
		}

		/// <summary>
		/// Gets the number of Cards in collection.
		/// </summary>
		public int Count
		{
			get
			{
				return this.cards.Count;
			}
		}

		public VirgilBuffer EncryptForAll(VirgilBuffer buffer)
		{
			throw new NotImplementedException();
		}

		public VirgilBuffer EncryptFor(Func<VirgilCard, bool> predicate, VirgilBuffer buffer)
		{
			throw new NotImplementedException();
		}
	}
}