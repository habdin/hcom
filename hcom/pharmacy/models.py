from django.db import models


class Company(models.Model):
    name = models.CharField(max_length=70, null=False)
    launch_date = models.DateField("Launch Date")
    history = models.TextField(max_length=10000)

    def __str__(self):
        return self.name

    class Meta:
        verbose_name_plural = "Companies"


class Dose_Unit(models.Model):
    name = models.CharField(max_length=99, null=False, blank=False)
    abbrev_name = models.CharField("Short Name", max_length=10, null=False, blank=False)

    def __str__(self):
        return self.abbrev_name


class Drug_Form(models.Model):
    name = models.CharField(max_length=50, null=False)
    abbrev = models.CharField(max_length=10, default="")

    def __str__(self):
        return self.name


class Scientific_Drug(models.Model):
    name = models.CharField(max_length=70, null=False)
    dose = models.DecimalField("Dose", max_digits=6, decimal_places=2, default=0)
    lethal_dose = models.DecimalField(
        "Lethal Dose", max_digits=6, decimal_places=2, default=0
    )
    drug_unit = models.ForeignKey(Dose_Unit, on_delete=models.CASCADE, default="")

    def __str__(self):
        return self.name


class Drug(models.Model):
    drug_name = models.CharField(max_length=255, blank=False)
    drug_dose = models.DecimalField("Dose", decimal_places=2, max_digits=6, default=0)
    drug_unit = models.ForeignKey(Dose_Unit, on_delete=models.CASCADE, default="")
    drug_form = models.ForeignKey(Drug_Form, on_delete=models.CASCADE, default="")
    drug_price = models.DecimalField("Price", decimal_places=2, max_digits=8, default=0)
    release_date = models.DateField("Release Date")
    expiry_date = models.DateField("Expiry Date")
    company = models.ForeignKey(Company, on_delete=models.CASCADE)

    def __str__(self):
        return self.drug_name


class Ingredient(models.Model):
    name = models.CharField(max_length=50, null=True)
    scientDrugID = models.ForeignKey(Scientific_Drug, on_delete=models.CASCADE)
    comDrugID = models.ForeignKey(Drug, on_delete=models.CASCADE)

    def __str__(self):
        return self.name
