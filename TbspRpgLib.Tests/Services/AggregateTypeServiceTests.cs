using Xunit;

using TbspRpgLib.Services;
using TbspRpgLib.Repositories;
using TbspRpgLib.Aggregates;

using System;

namespace TbspRpgLib.Tests.Services {
    public class AggregateTypeServiceTests {
        private AggregateTypeService _aggregateTypeService;

        public AggregateTypeServiceTests() {
            _aggregateTypeService = new AggregateTypeService(new AggregateTypeRepository());
        }

        [Fact]
        public void GetAggregateTypeName_Valid_GetsName() {
            //arrange
            //act
            string typeName = _aggregateTypeService.GetAggregateTypeName("content_87656335-9169-4314-b1e2-c4568b59ebf9");
            
            //assert
            Assert.Equal(AggregateService.CONTENT_AGGREGATE_TYPE, typeName);
        }

        [Fact]
        public void GetAggregateTypeName_Default_GetsName() {
            //arrange
            //act
            string typeName = _aggregateTypeService.GetAggregateTypeName("87656335-9169-4314-b1e2-c4568b59ebf9");
            
            //assert
            Assert.Equal(AggregateTypeRepository.DEFAULT_AGGREGATE_TYPE, typeName);
        }

        [Fact]
        public void GetAggregateTypeName_InValid_Exception() {
            //arrange
            //act
            //assert
            var exception = Assert.Throws<ArgumentException>(() => 
                _aggregateTypeService.GetAggregateTypeName("conten_87656335-9169-4314-b1e2-c4568b59ebf9"));
        }

        [Fact]
        public void GetPrefixForAggregateType_Valid_ReturnPrefix() {
            //act
            string prefix = _aggregateTypeService.GetPrefixForAggregateType(
                AggregateService.CONTENT_AGGREGATE_TYPE
            );

            //assert
            Assert.Equal(AggregateTypeRepository.CONTENT_AGGREGATE_PREFIX, prefix);
        }

        [Fact]
        public void GetPrefixForAggregateType_Valid_EmptyPrefix() {
            //act
            string prefix = _aggregateTypeService.GetPrefixForAggregateType(
                AggregateService.GAME_AGGREGATE_TYPE
            );

            //assert
            Assert.Null(prefix);
        }

        [Fact]
        public void GetPrefixForAggregateType_InValid_Exception() {
            var exception = Assert.Throws<ArgumentException>(() => 
                _aggregateTypeService.GetPrefixForAggregateType("grapes"));
        }

        [Fact]
        public void GenerateAggregateIdForAggregateType_Valid_ReturnPrefix() {
            //arrange
            string idwoprefix = "87656335-9169-4314-b1e2-c4568b59ebf9";
            //act
            string id = _aggregateTypeService.GenerateAggregateIdForAggregateType(
                idwoprefix,
                AggregateService.CONTENT_AGGREGATE_TYPE
            );

            //assert
            Assert.Equal(
                $"{AggregateTypeRepository.CONTENT_AGGREGATE_PREFIX}_{idwoprefix}", id);
        }

        [Fact]
        public void GenerateAggregateIdForAggregateType_Valid_EmptyPrefix() {
            //arrange
            string idwoprefix = "87656335-9169-4314-b1e2-c4568b59ebf9";
            //act
            string id = _aggregateTypeService.GenerateAggregateIdForAggregateType(
                idwoprefix,
                AggregateService.GAME_AGGREGATE_TYPE
            );

            //assert
            Assert.Equal(idwoprefix, id);
        }

        [Fact]
        public void GenerateAggregateIdForAggregateType_InValid_Exception() {
            //arrange
            string idwoprefix = "87656335-9169-4314-b1e2-c4568b59ebf9";
            //act,assert
            var exception = Assert.Throws<ArgumentException>(() => 
                _aggregateTypeService.GenerateAggregateIdForAggregateType(
                    idwoprefix,
                    "bananas"
            ));
        }

        [Fact]
        public void IsIdAlreadyPrefixed_HasPrefix_ReturnTrue() {
            string idwprefix = $"{AggregateTypeRepository.CONTENT_AGGREGATE_PREFIX}_87656335-9169-4314-b1e2-c4568b59ebf9";

            //act
            bool prefixed = _aggregateTypeService.IsIdAlreadyPrefixed(
                idwprefix, AggregateService.CONTENT_AGGREGATE_TYPE);
            
            Assert.True(prefixed);
        }


        [Fact]
        public void IsIdAlreadyPrefixed_HasNullPrefix_ReturnTrue() {
            string idwprefix = "87656335-9169-4314-b1e2-c4568b59ebf9";

            //act
            bool prefixed = _aggregateTypeService.IsIdAlreadyPrefixed(
                idwprefix, AggregateService.GAME_AGGREGATE_TYPE);
            
            Assert.True(prefixed);
        }


        [Fact]
        public void IsIdAlreadyPrefixed_NoPrefix_ReturnFalse() {
            string idwprefix = "87656335-9169-4314-b1e2-c4568b59ebf9";

            //act
            bool prefixed = _aggregateTypeService.IsIdAlreadyPrefixed(
                idwprefix, AggregateService.CONTENT_AGGREGATE_TYPE);
            
            Assert.False(prefixed);
        }

        [Fact]
        public void IsIdAlreadyPrefixed_WrongPrefix_Exception() {
            //arrange
            string idwprefix = "conten_87656335-9169-4314-b1e2-c4568b59ebf9";
            //act,assert
            var exception = Assert.Throws<Exception>(() => 
                _aggregateTypeService.IsIdAlreadyPrefixed(
                    idwprefix,
                    AggregateService.CONTENT_AGGREGATE_TYPE
                )
            );
        }

        [Fact]
        public void IsIdAlreadyPrefixed_WrongNullPrefix_Exception() {
            //arrange
            string idwprefix = "content_87656335-9169-4314-b1e2-c4568b59ebf9";
            //act,assert
            var exception = Assert.Throws<Exception>(() => 
                _aggregateTypeService.IsIdAlreadyPrefixed(
                    idwprefix,
                    AggregateService.GAME_AGGREGATE_TYPE
                )
            );
        }
    }
}