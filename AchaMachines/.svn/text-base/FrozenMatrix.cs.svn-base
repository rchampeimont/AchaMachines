// Copyright (c) 2009 Raphael Champeimont
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



namespace AchaMachines
{
	
	/// <summary>
	/// A non-mutable matrix.
	/// </summary>
	/// <remarks>
	/// IMPLEMENTATION STATUS: STABLE (you can assume the interface will not change,
	/// or will change but keep backward compatibility).
	/// </remarks>
	public class FrozenMatrix<T>
	{
		private T[,] matrix;
		
		/// <summary>
		/// This method enables you to access an element of the matrix using the M[i,j] syntax.
		/// </summary>
		public T this[int i, int j] {
			get {
				return matrix[i, j];
			}
		}
		
		/// <summary>
		/// Returns the length of the matrix.
		/// </summary>
		public int Length {
			get {
				return matrix.Length;
			}
		}
		
		public int GetLength(int dimension) {
			return matrix.GetLength(dimension);
		}
		
		/// <summary>
		/// Creates a frozen matrix from sourceMatrix. It makes a copy.
		/// You can then modify the source matrix without affecting the frozen matrix.
		/// </summary>
		/// <param name="sourceMatrix">
		/// The matrix used to build the frozen matrix as a copy.
		/// </param>
		public FrozenMatrix(T[,] sourceMatrix)
		{
			matrix = (T[,]) sourceMatrix.Clone();
		}
	}
}
