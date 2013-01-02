﻿using System;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenSegment : GenContainerFragmentBase
    {
        private GenBlock ItemBody { get; set; }
        private GenFragment Separator { get; set; }

        public GenSegment(GenDataDef genDataDef, string className, GenCardinality cardinality, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            Body.ParentSegement = this;
            FragmentType = FragmentType.Segment;
            ClassId = GenDataDef.Classes.IndexOf(className);
            GenCardinality = cardinality;
        }

        public GenCardinality GenCardinality { get; set; }

        public new int ClassId { get; set; }

        public override string ProfileLabel()
        {
            return GenDataDef.Classes[ClassId];
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[ClassId],
                                                 Body.ProfileText(syntaxDictionary),
                                                 syntaxDictionary.GenCardinalityText[(int) GenCardinality]
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            var s = new StringBuilder();
            bool isEmpty = true;
            string expanded;
            switch (GenCardinality)
            {
                case GenCardinality.All:
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        s.Append(Body.Expand(genData));
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter();
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        expanded = ItemBody.Expand(genData);
                        if (!isEmpty && expanded != "")
                            s.Append(Separator.Expand(genData));
                        isEmpty = isEmpty && expanded == "";
                        s.Append(expanded);
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.Back:
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        s.Append(Body.Expand(genData));
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter();
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        expanded = ItemBody.Expand(genData);
                        if (!isEmpty && expanded != "")
                            s.Append(Separator.Expand(genData));
                        isEmpty = isEmpty && expanded == "";
                        s.Append(expanded);
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.First:
                    genData.First(ClassId);
                    if (!genData.Eol(ClassId))
                        s.Append(Body.Expand(genData));
                    break;
                case GenCardinality.Tail:
                    genData.First(ClassId);
                    genData.Next(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        s.Append(Body.Expand(genData));
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    genData.Last(ClassId);
                    if (!genData.Eol(ClassId))
                        s.Append(Body.Expand(genData));
                    break;
                case GenCardinality.Trunk:
                    genData.Last(ClassId);
                    genData.Prior(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        s.Append(Body.Expand(genData));
                        genData.Prior(ClassId);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return s.ToString();
        }

        public override bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            var generated = false;
            bool generatedItem;
            bool isEmpty;
            GenBlock myPrefix;
            switch (GenCardinality)
            {
                case GenCardinality.All:
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generated |= Body.Generate(prefix, genData, writer);
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter();
                    isEmpty = true;
                    myPrefix = new GenBlock(GenDataDef, this);
                    if (prefix != null)
                        myPrefix.Body.Add(prefix);
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generatedItem = ItemBody.Generate(myPrefix, genData, writer);
                        if (isEmpty && generatedItem)
                        {
                            isEmpty = false;
                            if (Separator != null)
                                myPrefix.Body.Add(Separator);
                        }
                        genData.Next(ClassId);
                    }
                    generated = !isEmpty;
                    break;
                case GenCardinality.Back:
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generated |= Body.Generate(prefix, genData, writer);
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter();
                    isEmpty = true;
                    myPrefix = new GenBlock(GenDataDef, this);
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generatedItem = ItemBody.Generate(myPrefix, genData, writer);
                        if (isEmpty && generatedItem)
                        {
                            isEmpty = false;
                            if (Separator != null)
                                myPrefix.Body.Add(Separator);
                        }
                        genData.Prior(ClassId);
                    }
                    generated = !isEmpty;
                    break;
                case GenCardinality.First:
                    genData.First(ClassId);
                    if (!genData.Eol(ClassId))
                        generated |= Body.Generate(prefix, genData, writer);
                    break;
                case GenCardinality.Tail:
                    genData.First(ClassId);
                    genData.Next(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generated |= Body.Generate(prefix, genData, writer);
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    genData.Last(ClassId);
                    if (!genData.Eol(ClassId))
                        generated |= Body.Generate(prefix, genData, writer);
                    break;
                case GenCardinality.Trunk:
                    genData.Last(ClassId);
                    genData.Prior(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        generated |= Body.Generate(prefix, genData, writer);
                        genData.Prior(ClassId);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return generated;

        }

        private void CheckDelimiter()
        {
            if (Separator != null) return;
            
            // Optimization: This is done once when this method is first called
            Separator = Body.Count > 0 ? Body.Fragment[Body.Count - 1] : new GenNullFragment(GenDataDef, this);
            ItemBody = new GenBlock(GenDataDef, this);

            for (var i = 0; i < Body.Count - 1; i++)
                ItemBody.Body.Add(Body.Fragment[i]);
        }
    }
}