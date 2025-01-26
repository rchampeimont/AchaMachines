// Copyright (c) 2009 River Champeimont
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

// IMPLEMENTATION STATUS: STABLE (you can assume the interface will not change,
// or will change but keep backward compatibility).

namespace AchaMachines
{
	// Same principle as FrozenMatrix, but with dimension = 1 instead of 2.
	// See FrozenMatrix for documentation.
	public class FrozenVector<T>
	{
		private T[] vector;
		
		public T this[int i] {
			get {
				return vector[i];
			}
		}
		
		public int Length {
			get {
				return vector.Length;
			}
		}
		
		public FrozenVector(T[] sourceVector)
		{
			vector = (T[]) sourceVector.Clone();
		}
	}
}
