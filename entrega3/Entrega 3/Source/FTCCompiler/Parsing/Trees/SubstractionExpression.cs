﻿using FTCCompiler.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class SubstractionExpression : BinaryExpression
    {
        public SubstractionExpression()
        {
            Type = DataType.Integer;
        }
        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
