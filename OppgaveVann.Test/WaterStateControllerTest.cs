namespace OppgaveVann.Test
{
    public class WaterStateControllerTest
    {
        [Test]
        public void Test01WaterAt20Degrees()
        {
            var water = new Water(50, 20);
            Assert.AreEqual(WaterState.Fluid, water.State);
            Assert.AreEqual(20, water.Temperature);
            Assert.AreEqual(50, water.Amount);
        }
        [Test]
        public void Test02WaterAtMinus20Degrees()
        {
            var water = new Water(50, -20);
            Assert.AreEqual(WaterState.Ice, water.State);
            Assert.AreEqual(-20, water.Temperature);
        }
        [Test]
        // Tests whether the state becomes gas at 120 degrees 
        public void Test03WaterAt120Degrees()
        {
            var water = new Water(50, 120);
            Assert.AreEqual(WaterState.Gas, water.State);
            Assert.AreEqual(120, water.Temperature);
        }
        [Test]
        // At 0 and 100 degrees, we must enter an optional parameter to the constructor that indicates how much 
        // is in the first phase (i.e. ice when mixing ice and liquid - and liquid when mixing 
        // liquid and gas). This test checks that we get an exception if this is not specified and the temperature 
        // is 100 degrees.
        public void Test04WaterAt100DegreesWithoutProportion()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new Water(50, 100);
            });
            Assert.That(exception?.Message, Is.EqualTo("When temperature is 0 or 100, you must provide a value for proportion"));
        }
        [Test]
        // Checks that we get a mix of liquid and gas, with 30% of the first 
        public void Test05WaterAt100Degrees()
        {
            var water = new Water(50, 100, 0.3);
            Assert.AreEqual(WaterState.FluidAndGas, water.State);
            Assert.AreEqual(100, water.Temperature);
            Assert.AreEqual(0.3, water.ProportionFirstState);
        }
        [Test]
        public void Test06WaterAt100Degrees()
        {
            var water = new Water(50, 100, 0.3);
            Assert.AreEqual(WaterState.FluidAndGas, water.State);
            Assert.AreEqual(100, water.Temperature);
        }
        [Test]
        public void Test07WaterAt100Degrees()
        {
            var water = new Water(50, 100, 0.3);
            Assert.AreEqual(WaterState.FluidAndGas, water.State);
            Assert.AreEqual(100, water.Temperature);
        }
        [Test]
        // Tests that when we add energy, the temperature rises by the correct number of degrees 
        public void Test08AddEnergy1()
        {
            var water = new Water(4, 10);
            water.AddEnergy(10);
            Assert.AreEqual(12.5, water.Temperature);
        }
        [Test]
        public void Test09AddEnergy2()
        {
            var water = new Water(4, -10);
            water.AddEnergy(10);
            Assert.AreEqual(-7.5, water.Temperature);
        }
        [Test]
        // Tests that water below 0 degrees is both heated to 0 and then melted if we add enough energy.
        // Also tests that the temperature stops at 0 degrees if we don't have enough energy to melt everything. 
        public void Test10AddEnergy3()
        {
            var water = new Water(4, -10);
            water.AddEnergy(168);
            Assert.AreEqual(0, water.Temperature);
            Assert.AreEqual(WaterState.IceAndFluid, water.State);
            Assert.AreEqual(0.6, water.ProportionFirstState);
        }
        [Test]
        public void Test11AddEnergy4()
        {
            var water = new Water(4, -10);
            water.AddEnergy(360);
            Assert.AreEqual(0, water.Temperature);
            Assert.AreEqual(WaterState.Fluid, water.State);
        }
        [Test]
        // Tests that excess energy after melting goes to heating with the correct number of degrees.
        public void Test12AddEnergy5()
        {
            var water = new Water(4, -10);
            water.AddEnergy(400);
            Assert.AreEqual(10, water.Temperature);
            Assert.AreEqual(WaterState.Fluid, water.State);
        }
        [Test]
        public void Test13FluidToGasA()
        {
            var water = new Water(10, 70);
            water.AddEnergy(900);
            Assert.AreEqual(100, water.Temperature);
            Assert.AreEqual(WaterState.FluidAndGas, water.State);
            Assert.AreEqual(0.9, water.ProportionFirstState);
        }
        [Test]
        public void Test14FluidToGasB()
        {
            var water = new Water(10, 70);
            water.AddEnergy(6300);
            Assert.AreEqual(100, water.Temperature);
            Assert.AreEqual(WaterState.Gas, water.State);
        }
        [Test]
        public void Test14FluidToGasC()
        {
            var water = new Water(10, 70);
            water.AddEnergy(6400);
            Assert.AreEqual(110, water.Temperature);
            Assert.AreEqual(WaterState.Gas, water.State);
        }
    }
}