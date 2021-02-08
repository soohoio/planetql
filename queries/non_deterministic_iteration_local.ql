/**
 * @name Non-deterministic iteration detection
 * @description finds for-each iteration on unordered collection variable
 * @kind problem
 * @problem.severity warning
 * @id cs/non-deterministic-iteration
 * @tags maintainability
 * @precision medium
 */

import csharp

class UnorderedCollection extends Assignable {
  string type;

  UnorderedCollection() {
    type = this.getType().getUndecoratedName() and isUnorderedCollectionType(type)
  }
}

predicate isUnorderedCollectionType(string type) {
  type = "Hashtable"
  or
  type = "HashSet"
  or
  type = "Dictionary"
  or
  type = "IDictionary"
  or
  type = "ConcurrentBag"
  or
  type = "ConcurrentDictionary"
  or
  type = "Lookup"
  or
  type = "ILookup"
}

class OrderingMethod extends Method {
  OrderingMethod() { this.hasName("OrderBy") or this.hasName("OrderByDescending") }
}

class UnorderedCollectionLoop extends ForeachStmt {
  UnorderedCollectionLoop() {
    isIterableUnorderedType(this)
    or
    not isIterableUnorderedType(this) and
    hasUnorderedCollectionAccess(this) and
    not hasOrderingMethodCall(this)
  }
}

predicate isIterableUnorderedType(ForeachStmt loop) {
  exists(string type | type = loop.getIterableExpr().getType().getUndecoratedName() |
    isUnorderedCollectionType(type)
  )
}

predicate hasUnorderedCollectionAccess(ForeachStmt loop) {
  exists(AssignableAccess acs, UnorderedCollection uc |
    acs = getAssignableAccess(loop) and acs.getTarget() = uc
  )
}

AssignableAccess getAssignableAccess(ForeachStmt loop) {
  exists(Element e | loop.getIterableExpr().getAChild*() = e |
    e instanceof AssignableAccess and result = e
  )
}

predicate hasOrderingMethodCall(ForeachStmt loop) {
  exists(MethodCall mc |
    loop.getIterableExpr().getAChild*() = mc and mc.getTarget() instanceof OrderingMethod
  )
}

from UnorderedCollectionLoop loop, Expr expr
where expr = loop.getIterableExpr()
select 
  expr.getLocation() as Source_Location,
  expr as Iterable_Expression,
  expr.getType() as Type
