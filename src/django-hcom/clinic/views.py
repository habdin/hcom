# vim:foldmethod=indent:ts=4
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from typing import Any

from django.db.models import Q, QuerySet

# from django.shortcuts import render
from django.http import JsonResponse
from django.urls import reverse_lazy
from django.views import generic
from django.conf import settings

from .forms import ClinicForm
from .models import Appointment, Clinic, Patient, Physician, Speciality

# def index(request):
#    """
#    View for clinic manager home page.
#    """
#    clinics = Clinic.objects.all()
#    #patients = [
#    #    {
#    #        'Name': 'Richard Johnson',
#    #        'Time': '6:30 PM',
#    #    },
#    #    {
#    #        'Name': 'Mary Antoinette',
#    #        "Time": '7:00 PM',
#    #    }
#    #]
#    context = {
#        'title': 'Clinic Manager',
#        'clinics': clinics,
#        #'patients': patients,
#        #'doctor_name': "Hassan Abdin"
#    }
#    return render(request, 'clinic/index.html', context)


class ClinicListView(generic.ListView):
    model: type[Clinic] = Clinic
    context_object_name: str = "objects"
    paginate_by: int = 10

    def get_context_data(self, **kwargs):
        view_card: bool = True
        if self.request.GET.get("is_table"):
            view_card: bool = False
        context: dict[str, Any] = super().get_context_data(**kwargs)
        context["is_table"] = view_card
        context["title"] = "Clinics list"
        context["debug"] = settings.DEBUG
        return context


class ClinicSearchListView(generic.ListView):
    model: type[Clinic] = Clinic
    context_object_name: str = "objects"
    template_name: str = "clinic/card.html"

    def get_queryset(self, search: str | None = None):
        search = self.request.GET.get("search")
        if search:
            queryset = Clinic.objects.filter(
                Q(physician__first_name__icontains=search)
                | Q(physician__last_name__icontains=search)
            )
        else:
            queryset = super().get_queryset()
        return queryset


class ClinicSummaryView(generic.DetailView):
    model: type[Clinic] = Clinic
    template_name = "clinic/clinic_summary.html"

    def get_context_data(self, **kwargs):
        context: dict[str, Any] = super().get_context_data(**kwargs)
        context["appointment_list"] = Appointment.objects.all()
        context["patient_list"] = Patient.objects.all()
        return context


class ClinicDetailView(generic.DetailView):
    model: type[Clinic] = Clinic

    def get_context_data(self, **kwargs):
        context: dict[str, Any] = super().get_context_data(**kwargs)
        context["appointment_list"] = Appointment.objects.all()
        context["patient_list"] = Patient.objects.all()
        return context


class ClinicCreateView(generic.CreateView):
    model: type[Clinic] = Clinic
    form_class: type[ClinicForm] = ClinicForm


class ClinicUpdateView(generic.UpdateView):
    model: type[Clinic] = Clinic
    form_class: type[ClinicForm] = ClinicForm
    success_url: str = reverse_lazy("clinic:clinic-list")


class ClinicDeleteView(generic.DeleteView):
    model: type[Clinic] = Clinic
    success_url: str = reverse_lazy("clinic:clinic-list")


def load_data(request):
    """Load Model data as Json into a jquery DataTable and provides the server-side searching and
    filtering for the table.
    """
    if request.method == "GET":
        draw: str = request.GET.get("draw")
    else:
        draw: str = request.POST.get("draw")
    clinics: QuerySet[Clinic] = Clinic.objects.all()
    count: int = len(clinics)
    data: list = []
    for clinic in clinics:
        full_name: str = f"{clinic.physician.first_name} {clinic.physician.last_name}"
        data.append(
            {
                "physician_name": full_name,
                "id": clinic.id,
                "opening_time": clinic.opening_time.strftime("%I:%M %p"),
                "closing_time": clinic.closing_time.strftime("%I:%M %p"),
                "is_archived": clinic.is_archived,
            }
        )
    response: dict[str, Any] = {
        "draw": draw,
        "data": data,
        "recordsTotal": count,
        "recordsFiltered": count,
    }
    return JsonResponse(response)
