// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using Silk.NET.OpenGL;

namespace osu.Game.Graphics.OpenGL.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionAndColourVertex : IEquatable<PositionAndColourVertex>, IVertex
    {
        [VertexMember(2, VertexAttribPointerType.Float)]
        public Vector2 Position;

        [VertexMember(4, VertexAttribPointerType.Float)]
        public Colour4 Colour;

        public bool Equals(PositionAndColourVertex other)
            => Position.Equals(other.Position)
               && Colour.Equals(other.Colour);
    }
}
