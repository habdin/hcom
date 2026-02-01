# vim:foldmethod=indent:ts=2
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django import forms

from .models import Drug, Company


class DrugForm(forms.ModelForm):
    class Meta:
        model = Drug
        fields = [
            "drug_name",
            "drug_dose",
            "drug_unit",
            "drug_form",
            "drug_price",
            "company",
        ]


class CompanyForm(forms.ModelForm):
    class Meta:
        model = Company
        fields = [
            "name",
            "launch_date",
            "history",
        ]
