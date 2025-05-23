﻿using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>
        : ShouldRelateToTypesThat<TRuleTypeShouldConjunction, Attribute, TRuleType>,
            IAttributePredicates<TRuleTypeShouldConjunction, Attribute>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        public ShouldRelateToAttributesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(AttributePredicatesDefinition.AreAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreSealed()
        {
            _ruleCreator.ContinueComplexCondition(AttributePredicatesDefinition.AreSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations

        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(AttributePredicatesDefinition.AreNotAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotSealed()
        {
            _ruleCreator.ContinueComplexCondition(AttributePredicatesDefinition.AreNotSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
