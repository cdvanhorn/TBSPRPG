using Xunit;

using TbspRpgLib.Services;
using TbspRpgLib.Repositories;

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
            Assert.Equal(AggregateTypeRepository.CONTENT_AGGREGATE_TYPE, typeName);
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
                AggregateTypeRepository.CONTENT_AGGREGATE_TYPE
            );

            //assert
            Assert.Equal(AggregateTypeRepository.CONTENT_AGGREGATE_PREFIX, prefix);
        }

        [Fact]
        public void GetPrefixForAggregateType_Valid_EmptyPrefix() {
            //act
            string prefix = _aggregateTypeService.GetPrefixForAggregateType(
                AggregateTypeRepository.GAME_AGGREGATE_TYPE
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
                AggregateTypeRepository.CONTENT_AGGREGATE_TYPE
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
                AggregateTypeRepository.GAME_AGGREGATE_TYPE
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
    }
}