﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLFace
{
    public MDLVertex Vertex1 { get; set; } = new();
    public MDLVertex Vertex2 { get; set; } = new();
    public MDLVertex Vertex3 { get; set; } = new();

    public float PlaneDistance { get; set; }
    public SurfaceMaterial Material { get; set; }
    public Vector3 Normal { get; set; } = new();

    // TODO - Adjacent Faces
}
